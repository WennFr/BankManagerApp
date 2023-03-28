using BankManagerApp.NorthwindData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankManagerApp.Pages
{
    public class SuppliersModel : PageModel
    {

		private readonly NorthwindInclIdentityContext _dbContext;

		public SuppliersModel(NorthwindInclIdentityContext dbContext)
		{
			_dbContext = dbContext;
		}

		public class SupplierViewModel
		{

			public int Id { get; set; }
			public string Name { get; set; }
			public string Country { get; set; }
			public string City { get; set; }
		}

		public List<SupplierViewModel> Suppliers { get; set; }



		public void OnGet(string sortColumn, string sortOrder)
        {

            var query = _dbContext.Suppliers.Select(s => new SupplierViewModel
            {
                Id = s.SupplierId,
                Name = s.CompanyName,
                City = s.City,
                Country = s.Country
            });

            if (sortColumn == "Name")
                if (sortOrder == "asc")
                    query = query.OrderBy(s => s.Name);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(s => s.Name);

            if (sortColumn == "Country")
                if (sortOrder == "asc")
                    query = query.OrderBy(s => s.Country);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(s => s.Country);

            if (sortColumn == "City")
                if (sortOrder == "asc")
                    query = query.OrderBy(s => s.City);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(s => s.City);


            Suppliers = query.ToList();



        }
    }
}
