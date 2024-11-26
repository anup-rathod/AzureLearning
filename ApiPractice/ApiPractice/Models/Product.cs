namespace ApiPractice.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
       
        public int ProductID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }

}
