using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpsy200FinalProject.Data
{
    public class Category : Equipment
    {
        private int categoryId;
        private string description;

        public int CategoryId { get => categoryId; set => categoryId = value; }
        public string Description { get => description; set => description = value; }

        public Category()
        {
             
        }

        public Category(int categoryId, string description)
        {
            this.categoryId = categoryId;
            this.description = description;
        }
    }
}
