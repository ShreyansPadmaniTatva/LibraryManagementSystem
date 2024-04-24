using LibraryManagement.Entities.DataContext;
using LibraryManagement.Entities.DataModels;
using LibraryManagement.Entities.ViewModel;
using LibraryManagement.Repositories.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LibraryManagement.Repositories.Repositories
{
    public class BookRepositories : IBookRepositories
    {
        private readonly ApplicationDbContext _context;

        public BookRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public BookModel GetBooks(BookModel bm)
        {
            BookModel dm = new BookModel();



            List<ViewBook> allData = (from book in _context.Books
                                      join borrower in _context.Borrowers
                                      on book.Borrowerid equals borrower.Borrowerid into BookGroup
                                      from borrowers in BookGroup.DefaultIfEmpty()
                                      where  (book.Isdeleted == false)
                                      orderby book.Createddate
                                      select new ViewBook
                                      {
                                          Bookname = book.Bookname,
                                          Isdeleted = false,
                                          Borrowerid = book.Borrowerid,
                                          Author = book.Author,
                                          Bookid = book.Bookid,
                                          BorrowerName = borrowers.Borrowername,
                                          City = book.City,
                                          Dateofissue = book.Dateofissue,
                                          Genere = book.Genere,
                                          Createddate = book.Createddate,

                                      }).OrderBy(x => x.Createddate).ToList();


            if (bm.IsAscending == true)
            {
                allData = bm.SortedColumn switch
                {
                    "Bookname" => allData.OrderBy(x => x.Bookname).ToList(),
                    "Author" => allData.OrderBy(x => x.Author).ToList(),
                    _ => allData.OrderBy(x => x.Createddate).ToList()
                };
            }
            else
            {
                allData = bm.SortedColumn switch
                {
                    "Bookname" => allData.OrderByDescending(x => x.Bookname).ToList(),
                    "Author" => allData.OrderByDescending(x => x.Author).ToList(),
                    _ => allData.OrderByDescending(x => x.Createddate).ToList()
                };
            }

            dm.TotalPages = (int)Math.Ceiling((double)allData.Count() / bm.PageSize);
            dm.BookList = allData.Skip((bm.CurrentPage - 1) * bm.PageSize).Take(bm.PageSize).ToList();
            dm.PageSize = bm.PageSize;
            dm.CurrentPage = bm.CurrentPage;
            dm.SortedColumn = bm.SortedColumn;
            dm.IsAscending = bm.IsAscending;
            return dm;
        }

        public async Task<bool> AddBook(ViewBook vb)
        {
            try
            {
                var borrower = await IsBorrowerExist(vb.BorrowerName.Trim() != null ? vb.BorrowerName.Trim() : null);
                if (borrower != null)
                {
                    Book book = new Book();
                    book.Author = vb.Author;
                    book.Bookname = vb.Bookname;
                    book.Dateofissue = vb.Dateofissue;
                    book.City = vb.City;
                    book.Genere = vb.Genere;
                    book.Isdeleted = false;
                    book.Borrowerid = borrower.Borrowerid;
                    book.Createddate = DateTime.Now;
                    _context.Books.Add(book);
                    _context.SaveChanges();
                }
                else
                {
                    Borrower borrower1 = new Borrower();
                    borrower1.Borrowername = vb.BorrowerName.Trim();
                    borrower1.City = vb.City;
                    borrower1.Createddate = DateTime.Now;
                    _context.Borrowers.Add(borrower1);
                    _context.SaveChanges();

                    Book book = new Book();
                    book.Author = vb.Author;
                    book.Bookname = vb.Bookname;
                    book.Dateofissue = vb.Dateofissue;
                    book.City = vb.City;
                    book.Genere = vb.Genere;
                    book.Createddate = DateTime.Now;
                    book.Isdeleted = false;
                    book.Borrowerid = borrower1.Borrowerid;
                    _context.Books.Add(book);
                    _context.SaveChanges();

                }
                return true;

            }
            catch (Exception ex)
            {

            }
            return true;
        }

        public async Task<bool> EditBook(ViewBook vb)
        {
            try
            {
                var borrower = await IsBorrowerExist(vb.BorrowerName.Trim() != null ? vb.BorrowerName.Trim() : null);
                if (borrower != null)
                {
                    Book book = _context.Books.Where(r => r.Bookid == vb.Bookid).FirstOrDefault();
                    book.Author = vb.Author;
                    book.Bookname = vb.Bookname;
                    book.Dateofissue = vb.Dateofissue;
                    book.City = vb.City;
                    book.Genere = vb.Genere;
                    book.Borrowerid = borrower.Borrowerid;
                    book.Modifieddate = DateTime.Now;
                    _context.Books.Update(book);
                    _context.SaveChanges();
                }
                else
                {
                    Borrower borrower1 = new Borrower();
                    borrower1.Borrowername = vb.BorrowerName.Trim();
                    borrower1.City = vb.City;
                    borrower1.Createddate = DateTime.Now;
                    _context.Borrowers.Add(borrower1);
                    _context.SaveChanges();

                    Book book = _context.Books.Where(r => r.Bookid == vb.Bookid).FirstOrDefault();
                    book.Author = vb.Author;
                    book.Bookname = vb.Bookname;
                    book.Dateofissue = vb.Dateofissue;
                    book.City = vb.City;
                    book.Genere = vb.Genere;
                    book.Modifieddate = DateTime.Now;
                    book.Borrowerid = borrower1.Borrowerid;
                    _context.Books.Update(book);
                    _context.SaveChanges();

                }
                return true;

            }
            catch (Exception ex)
            {

            }
            return true;
        }

        public async Task<Borrower> IsBorrowerExist(string name)
        {
            return await _context.Borrowers.Where(r => r.Borrowername.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }

        public ViewBook GetBookById(int BookId)
        {
            var v = (from book in _context.Books
                     join borrower in _context.Borrowers
                     on book.Borrowerid equals borrower.Borrowerid into BookGroup
                     from borrowers in BookGroup.DefaultIfEmpty()
                     where (book.Bookid == BookId)
                     orderby book.Createddate
                     select new ViewBook
                     {
                         Bookname = book.Bookname,
                         Isdeleted = false,
                         Borrowerid = book.Borrowerid,
                         Author = book.Author,
                         Bookid = book.Bookid,
                         BorrowerName = borrowers.Borrowername,
                         City = book.City,
                         Dateofissue = book.Dateofissue,
                         Genere = book.Genere,
                         Createddate = book.Createddate,

                     })
                .FirstOrDefault(); 
            return v;
        }

        public async Task<bool> RemoveBook(int BookId)
        {
            try
            {
                    Book book = _context.Books.Where(r => r.Bookid == BookId).FirstOrDefault();
                if (book != null)
                {
                    book.Isdeleted = true;
                    _context.Books.Update(book);
                    _context.SaveChanges();
                }
                return true;

            }
            catch (Exception ex)
            {

            }
            return true;
        }
    }
}
