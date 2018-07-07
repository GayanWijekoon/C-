using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using MVCWEF.Models;

namespace MVCWEF.Controllers
{
    public class ProductController : Controller
    {
        private String connectionString = @"Data Source=.;Initial Catalog=MvcCrud;Integrated Security=True";

        [HttpGet]
        public ActionResult Index()
            {
                DataTable dtTable = new DataTable();
                using (SqlConnection sqlCon=new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    //SqlDataAdapter sqlDa=new SqlDataAdapter("select branch.branchid,branch.branchname,customer.id,customer.name from branch inner join customer ON customer.id=branch.cid", sqlCon);
                    SqlDataAdapter sqlDa = new SqlDataAdapter("select * from customer", sqlCon);
                sqlDa.Fill(dtTable);
                }
            return View(dtTable);
        }

        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Login(CustomerModel customerModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                //String mainconn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                //SqlConnection sqlconn = new SqlConnection(mainconn);

                sqlCon.Open();
                String sqlquery = "select name,description from customer where name=@name and description=@psw";



                SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlCon);
                sqlcomm.Parameters.AddWithValue("@name", customerModel.name);
                sqlcomm.Parameters.AddWithValue("@psw", customerModel.description);
                SqlDataReader sdr = sqlcomm.ExecuteReader();
                if (sdr.Read())
                {
                    Session["username"] = customerModel.name.ToString();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Message"] = "login failed";
                }

                sqlCon.Close();
            }

            return View();
        }


        // GET: Product/Create
        public ActionResult Create()
        {
            return View(new CustomerModel());
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(CustomerModel customerModel)
        {
            using (SqlConnection sqlCon=new SqlConnection(connectionString))
            {
                sqlCon.Open();
                String query = "insert into customer values(@CustomerName,@CustomerDescription)";
                SqlCommand sqlCmd=new SqlCommand(query,sqlCon);
                sqlCmd.Parameters.AddWithValue("@CustomerName", customerModel.name);
                sqlCmd.Parameters.AddWithValue("@CustomerDescription", customerModel.description);

                sqlCmd.ExecuteNonQuery();

            }
            return RedirectToAction("Index");
        }



        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            CustomerModel customerModel=new CustomerModel();
            DataTable dataTable=new DataTable();
            using (SqlConnection sqlCon=new SqlConnection(connectionString))
            {
                sqlCon.Open();
                String query = "select * from customer where id =@customerID";
                SqlDataAdapter sqlData=new SqlDataAdapter(query,sqlCon);
                sqlData.SelectCommand.Parameters.AddWithValue("@customerID", id);
                sqlData.Fill(dataTable);

            }

            if (dataTable.Rows.Count == 1)
            {
                customerModel.id=Convert.ToInt32(dataTable.Rows[0][0].ToString());
                customerModel.name = dataTable.Rows[0][1].ToString();
                customerModel.description = dataTable.Rows[0][2].ToString();
                return View(customerModel);
            }
            else
            return RedirectToAction("Index");
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(CustomerModel customerModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                String query = "Update customer set name=@CustomerName,description=@CustomerDescription where id=@CustomerID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@CustomerName", customerModel.name);
                sqlCmd.Parameters.AddWithValue("@CustomerDescription", customerModel.description);
                sqlCmd.Parameters.AddWithValue("@CustomerID", customerModel.id);

                sqlCmd.ExecuteNonQuery();

            }
            return RedirectToAction("Index");
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                String query = "delete from customer where id=@CustomerID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
               
                sqlCmd.Parameters.AddWithValue("@CustomerID", id);

                sqlCmd.ExecuteNonQuery();

            }
            return RedirectToAction("Index");
        }

        


    }
}
