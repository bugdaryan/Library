using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Models.Branch;
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
            var branches = _branch.GetAll().Select(branch => new BranchDetailModel
            {
                Id = branch.Id,
                Address = branch.Address,
                Description = branch.Description,
                HoursOpen = _branch.GetBranchHours(branch.Id),
                ImageUrl = branch.ImageUrl,
                IsOpen = _branch.IsBranchOpen(branch.Id),
                Name = branch.Name,
                NumberOfAssets = branch.LibraryAssets.Count(),
                NumberOfPatrons = branch.Patrons.Count(),
                OpenDate = branch.OpenDate.ToString("yyyy-MM-dd"),
                Telephone = branch.Telephone,
                TotalAssetValue = branch.LibraryAssets.Sum(asset => asset.Cost)
            });

            var model = new BranchIndexModel
            {
                Branches = branches
            };
            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var branch = _branch.Get(id);
            var model = new BranchDetailModel
            {
                Id = branch.Id,
                Address = branch.Address,
                Description = branch.Description,
                HoursOpen = _branch.GetBranchHours(id),
                ImageUrl = branch.ImageUrl,
                IsOpen = _branch.IsBranchOpen(id),
                Name = branch.Name,
                NumberOfAssets = branch.LibraryAssets.Count(),
                NumberOfPatrons = branch.Patrons.Count(),
                OpenDate = branch.OpenDate.ToString("yyyy-MM-dd"),
                Telephone = branch.Telephone,
                TotalAssetValue = branch.LibraryAssets.Sum(asset => asset.Cost)
            };
            return View(model);
        }
    }
}