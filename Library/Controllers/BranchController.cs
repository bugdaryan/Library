using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryData;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class BranchController : Controller
    {
        private readonly ILibraryBranch _branch;
        public BranchController(ILibraryBranch branch)
        {
            _branch = branch;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}