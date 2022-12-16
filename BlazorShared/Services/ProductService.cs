using BlazorShared.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BlazorShared.Services
{
    public class ProductService : IProductService
    {
        private readonly SqlConnectionConfiguration _configuration;
        public ProductService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> AddProduct(Product product)
        {

            using (SqlConnection con = new SqlConnection(_configuration.Value))
            {
                const string query = "insert into dbo.CatalogProduct (Id, Barcode, ProductName, Quantity) values (@Id, @Barcode, @ProductName, @Quantity)";
                SqlCommand cmd = new SqlCommand(query, con)
                {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
                cmd.Parameters.AddWithValue("@Barcode", product.Barcode);
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);

                con.Open();
                await cmd.ExecuteNonQueryAsync();

                con.Close();
                cmd.Dispose();
            }
            return true;
        }

        public async Task<List<Product>> GetProductList()
        {
            List<Product> productList = new List<Product>();

            using (SqlConnection con = new SqlConnection(_configuration.Value))
            {
                const string query = "select * from dbo.CatalogProduct";
                SqlCommand cmd = new SqlCommand(query, con)
                {
                    CommandType = CommandType.Text
                };

                con.Open();
                SqlDataReader rdr = await cmd.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    Product product = new Product
                    {
                        Id = rdr["Id"].ToString(),
                        Barcode = rdr["Barcode"].ToString(),
                        ProductName = rdr["ProductName"].ToString(),
                        Quantity = Convert.ToSingle(rdr["Quantity"]),
                    };
                    productList.Add(product);
                }
                con.Close();
                cmd.Dispose();
            }
            return productList;
        }

        public async Task<Product> GetProduct(string barcodeText)
        {
            Product product = new Product();

            using (SqlConnection con = new SqlConnection(_configuration.Value))
            {
                const string query = "select * from CatalogProduct where Barcode = @Barcode";
                SqlCommand cmd = new SqlCommand(query, con)
                {
                    CommandType = CommandType.Text,
                };

                cmd.Parameters.AddWithValue("@Barcode", barcodeText);
                con.Open();

                SqlDataReader rdr = await cmd.ExecuteReaderAsync();

                if (rdr.Read())
                {
                    product.Id = rdr["Id"].ToString();
                    product.Barcode = rdr["Barcode"].ToString();
                    product.ProductName = rdr["ProductName"].ToString();
                    product.Quantity = Convert.ToSingle(rdr["Quantity"]);

                }

                con.Close();
                cmd.Dispose();
            }
            return product;
        }
    }
}