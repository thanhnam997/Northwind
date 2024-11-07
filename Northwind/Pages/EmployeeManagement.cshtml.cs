using Microsoft.AspNetCore.Mvc;

   using Microsoft.AspNetCore.Mvc.RazorPages;

   using Microsoft.AspNetCore.Mvc.Rendering;

   using Microsoft.Extensions.Configuration;

   using System.Data.SqlClient;

 

   namespace Module10Lab.Pages

   {

       public class EmployeeManagementModel : PageModel

       {

           private readonly string _connectionString;

 

           public EmployeeManagementModel(IConfiguration configuration)

           {

               _connectionString = configuration.GetConnectionString("NorthwindConnection");

           }

 

           [BindProperty]

           public int? SelectedEmployeeId { get; set; }

 

           public List<SelectListItem> EmployeeList { get; set; }

           public dynamic SelectedEmployee { get; set; }

 

           public void OnGet()

           {

               LoadEmployeeList();

           }

 

           public IActionResult OnPost()

           {

               LoadEmployeeList();

               if (SelectedEmployeeId.HasValue)

               {

                   SelectedEmployee = GetEmployeeById(SelectedEmployeeId.Value);

               }

               return Page();

           }

 

           public IActionResult OnPostUpdate(int EmployeeID, string Title, string City)

           {

               UpdateEmployee(EmployeeID, Title, City);

               return RedirectToPage();

           }

 

           public IActionResult OnPostDelete(int EmployeeID)

           {

               DeleteEmployee(EmployeeID);

               return RedirectToPage();

           }

 

           public IActionResult OnPostAdd(string NewFirstName, string NewLastName, string NewTitle)

           {

               AddEmployee(NewFirstName, NewLastName, NewTitle);

               return RedirectToPage();

           }

 

           private void LoadEmployeeList()

           {

               EmployeeList = new List<SelectListItem>();

               using (SqlConnection connection = new SqlConnection(_connectionString))

               {

                   connection.Open();

                   string sql = "SELECT EmployeeID, FirstName, LastName FROM Employees";

                   using (SqlCommand command = new SqlCommand(sql, connection))

                   {

                       using (SqlDataReader reader = command.ExecuteReader())

                       {

                           while (reader.Read())

                           {

                               EmployeeList.Add(new SelectListItem

                               {

                                   Value = reader["EmployeeID"].ToString(),

                                   Text = $"{reader["FirstName"]} {reader["LastName"]}"

                               });

                           }

                       }

                   }

               }

           }

 

           private dynamic GetEmployeeById(int id)

           {

               using (SqlConnection connection = new SqlConnection(_connectionString))

               {

                   connection.Open();

                   string sql = "SELECT EmployeeID, FirstName, LastName, Title, City FROM Employees WHERE EmployeeID = @Id";

                   using (SqlCommand command = new SqlCommand(sql, connection))

                   {

                       command.Parameters.AddWithValue("@Id", id);

                       using (SqlDataReader reader = command.ExecuteReader())

                       {

                           if (reader.Read())

                           {

                               return new

                               {

                                   EmployeeID = (int)reader["EmployeeID"],

                                   FirstName = reader["FirstName"].ToString(),

                                   LastName = reader["LastName"].ToString(),

                                   Title = reader["Title"].ToString(),

                                   City = reader["City"].ToString()

                               };

                           }

                       }

                   }

               }

               return null;

           }

 

           private void UpdateEmployee(int employeeId, string title, string city)

           {

               using (SqlConnection connection = new SqlConnection(_connectionString))

               {

                   connection.Open();

                   string sql = "UPDATE Employees SET Title = @Title, City = @City WHERE EmployeeID = @EmployeeID";

                   using (SqlCommand command = new SqlCommand(sql, connection))

                   {

                       command.Parameters.AddWithValue("@EmployeeID", employeeId);

                       command.Parameters.AddWithValue("@Title", title);

                       command.Parameters.AddWithValue("@City", city);

                       command.ExecuteNonQuery();

                   }

               }

           }

 

           private void DeleteEmployee(int id)

           {

               using (SqlConnection connection = new SqlConnection(_connectionString))

               {

                   connection.Open();

                   string sql = "DELETE FROM Employees WHERE EmployeeID = @Id";

                   using (SqlCommand command = new SqlCommand(sql, connection))

                   {

                       command.Parameters.AddWithValue("@Id", id);

                       command.ExecuteNonQuery();

                   }

               }

           }

 

           private void AddEmployee(string firstName, string lastName, string title)

           {

               using (SqlConnection connection = new SqlConnection(_connectionString))

               {

                   connection.Open();

                   string sql = @"INSERT INTO Employees (FirstName, LastName, Title, BirthDate, HireDate, Address, City, Country)

                                  VALUES (@FirstName, @LastName, @Title, '1980-01-01', GETDATE(), '123 Main St', 'Anytown', 'USA')";

                   using (SqlCommand command = new SqlCommand(sql, connection))

                   {

                       command.Parameters.AddWithValue("@FirstName", firstName);

                       command.Parameters.AddWithValue("@LastName", lastName);

                       command.Parameters.AddWithValue("@Title", title);

                       command.ExecuteNonQuery();

                   }

               }

           }

       }

   }