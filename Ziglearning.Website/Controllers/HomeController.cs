using System.Linq;
using System.Web.Mvc;
using Ziglearning.Website.Models;
using Ziglearning.Business;

namespace Ziglearning.Website.Controllers
{
    public class HomeController : Controller
    {

        private readonly IClassManager classManager;
        private readonly IUserManager userManager;


        public HomeController(IClassManager classManager,
                              IUserManager userManager)
        {
            this.classManager = classManager;
            this.userManager = userManager;

        }

        public HomeController()
        {
        }

        public ActionResult ClassList()
        {
            var classes = classManager.Classes
                                        .Select(t => new Ziglearning.Website.Models.ClassModel(t.Id, t.Name, t.Description, t.Price))
                                        .ToArray();
            var model = new IndexModel { Classes = classes };
            return View(model);
        }


        public ActionResult Index()
        {
            var users = userManager.Users
                                            .Select(t => new Ziglearning.Website.Models.UserModel(t.Id, t.Name))
                                            .ToArray();
            var model = new IndexModel { Users = users };
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


       
        public ActionResult LogIn(string returnUrl)

        {
            RedirectToAction("login", "Home", new { Id = returnUrl }); 
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(LoginModel loginModel, string returnUrl)

        {
            if (ModelState.IsValid)
            {
                var user = userManager.LogIn(loginModel.Email, loginModel.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Email and password do not match.");
                }
                else
                {
                    Session["User"] = new Ziglearning.Website.Models.UserModel(user.Id, user.Name)
                    {
                        Id = user.Id,
                        Name = user.Name
                    };

                    System.Web.Security.FormsAuthentication.SetAuthCookie(
                        loginModel.Email, false);

                    return Redirect(returnUrl ?? "~/");
                }
            }

            return View(loginModel);
        }


        public ActionResult LogOff()
        {
            Session["User"] = null;

            System.Web.Security.FormsAuthentication.SignOut();

            return Redirect("~/");
        }

        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(RegisterModel registerModel, string returnUrl)
        {

            if (ModelState.IsValid)
            {
           
             var alreadyRegistered = userManager.AlreadyRegistered(registerModel.Email, registerModel.Password);


                if (alreadyRegistered != null)

                {
                    ModelState.AddModelError("", "You have already registered, please login instead.");

                }


                else
                {
                    if (registerModel.Email != "")

                        if (registerModel.Password != "")

                                    if (registerModel.Password == registerModel.ConfirmPassword)

                                    { var user = userManager.Register(registerModel.Email, registerModel.Password);

                                    Session["User"] = new Ziglearning.Website.Models.UserModel(user.Id, user.Name)
                                    {
                                    Id = user.Id,
                                    Name = user.Name
                                    };

                                    System.Web.Security.FormsAuthentication.SetAuthCookie(
                                    registerModel.Email, false);

                                    return Redirect(returnUrl ?? "~/");

                                    }

                                    else

                                    { ModelState.AddModelError("", "Passwords do not match."); }

                        else { ModelState.AddModelError("", "Password cannot be empty"); }

                    else { ModelState.AddModelError("", "Email cannot be empty"); }
                }

            RedirectToAction("Index");
            return View(registerModel);
            }
           else
            {
                ModelState.AddModelError("", "There is a validation error.");
                return View();
            }
        }

       

        [Authorize]
        public ActionResult StudentClasses(int id)
        {
        var user = (Ziglearning.Website.Models.UserModel)Session["User"];
      
             var clas = classManager
                                 .ForUser(id)
                                 .Select(t =>
                                     new Ziglearning.Website.Models.ClassModel(t.Id, t.Name, t.Description, t.Price)
                                     {
                                         Id = t.Id,
                                         Name = t.Name,
                                         Description = t.Description,
                                         Price = t.Price
                                     }).ToArray();

             var model = new UserViewModel
             {
                 User = new Ziglearning.Website.Models.UserModel(user.Id, user.Name),
                 Classes = clas
             };
             return View(model);
         }




        [Authorize]
        public ActionResult YourEnrolledClasses(string returnUrl)

        {
            if (Session["User"] == null)

            { return RedirectToAction("login", "Home", new { returnUrl = "YourEnrolledClasses" }); }

            else if (Session["User"] != null)

            {
                var user = (Ziglearning.Website.Models.UserModel)Session["User"];

                return RedirectToAction("StudentClasses", new { id = user.Id });
            }

            return Redirect(returnUrl ?? "~/");
        }



     //Called by the selection and submission of the drop-down list item

     [Authorize]
     [HttpPost]
        public ActionResult EnrollToAClass(FormCollection model)
        {
            if (ModelState.IsValid)

            {
                string returnUrl = "Enroll";

                if (Session["User"] == null)

                { return RedirectToAction("login", "Home", new { id = returnUrl }); }


                Models.UserModel user = (Ziglearning.Website.Models.UserModel)Session["User"];


                //*************************Selected Class Object Retrieved from Drop-Down List***********************

                string classId = model.GetValue("classes").AttemptedValue;//get Id string

                int classIdInt = !string.IsNullOrEmpty(classId) ? int.Parse(classId) : 0;//get id int

                Business.ClassModel chosenclas = classManager.Clas(classIdInt);//return classModel based on class id int



                //*************************Array of Classes Already Taken by that User*********************************

                var takenclasses = classManager.ForUser(user.Id);


                //**********************Loop Through Taken Classes Array and compare  with selected class Object

                int i;

                if (takenclasses.Length > 0)
                {

                    for (i = 0; i < takenclasses.Length; i++)

                    {
                        var x = takenclasses[i];
                        var y = chosenclas;

                        if (takenclasses[i].Id == chosenclas.Id)

                            return View("AlreadyTaken");

                    }

                    if (classIdInt > 0)
                    {
                        var enrolled = classManager.AddUserToClass(user.Id, classIdInt);
                        //return View("Thanks");
                        return RedirectToAction("StudentClasses", new { id = user.Id });

                    }

                }

                //********************* No Classes were already taken, so we enroll directly

                else if (takenclasses.Length == 0)
                {
                    if (classIdInt > 0)

                    {
                        var enrolled = classManager.AddUserToClass(user.Id, classIdInt);
                        //return View("Thanks");
                        var userfin = (Ziglearning.Website.Models.UserModel)Session["User"];
                        return RedirectToAction("StudentClasses", new { id = userfin.Id });

                    }
                }

             var userend = (Ziglearning.Website.Models.UserModel)Session["User"];

             return RedirectToAction("StudentClasses", new { id = userend.Id });
                    
            }

            else
            { 
               
               return View();
            }

        }
       


        // "Page" Containing the Classes Drop-Down list

        [Authorize]
        public ActionResult Enroll()
        {
            if (Session["User"] == null)

            {
                return RedirectToAction("login", "Home", new { returnUrl = "Enroll" });
            }

            var classes = classManager.Classes
                                      .Select(t => new Ziglearning.Website.Models.ClassModel(t.Id, t.Name, t.Description, t.Price))
                                      .ToArray();
            var model = new EnrollModel
            { Classes = classes };

            return View(model);
        }


        public ActionResult Error()
        {
            return View();
        }



    }
}

