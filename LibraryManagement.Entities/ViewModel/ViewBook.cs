using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Entities.ViewModel
{
    public class BookModel
    {
        public List<ViewBook>? BookList { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        //Extra Inputs
        public string? SearchInput { get; set; }
        public int? Borrowerid { get; set; }

        //sORTING
        public string SortedColumn { get; set; }
        public bool? IsAscending { get; set; }
    }


    public class ViewBook
    {
        public int Bookid { get; set; }
        [Required(ErrorMessage = "Bookname is required")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Enter valid Bookname")]
        [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid Book name")]
        public string Bookname { get; set; }

        [Required(ErrorMessage = "Author is required")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Enter valid Author")]
        [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid city name")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Enter valid Date")]
        public DateTime Dateofissue { get; set; }

        [Required(ErrorMessage = "Genere is required")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Enter valid Genere")]
        public short Genere { get; set; }

        
        public int Borrowerid { get; set; }
        public string? BorrowerName { get; set; } = null!;

        [Required(ErrorMessage = "City is required")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Enter valid City")]
        [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid city name")]
        public string City { get; set; }

       
        public bool? Isdeleted { get; set; }

        
        public DateTime Createddate { get; set; }

       
        public DateTime? Modifieddate { get; set; }
    }
}
