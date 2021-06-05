using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBlog.Models;
using PagedList;
using PagedList.Mvc;

namespace MVCBlog.Controllers
{
    public class BlogController : Controller
    {
        DBBlogEntities db = new DBBlogEntities();
        // GET: Blog
        public ActionResult Index(int page=1)
        {
            var blogs = db.Blogs.OrderByDescending(b => b.BlogID).ToPagedList(page, 2);
            return View(blogs);
        }

        public ActionResult Hakkinda()
        {
            var hakkinda = db.Hakkindas.ToList();
            return View(hakkinda);
        }

        public ActionResult BlogDetay(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var blogDetail = db.Blogs.Where(b => b.BlogID == id).SingleOrDefault();

            if (blogDetail == null)
            {
                return HttpNotFound();
            }
            return View(blogDetail);
            

        }

        public ActionResult OkumaArttir(int blogid)
        {
            var blog = db.Blogs.Where(b => b.BlogID == blogid).SingleOrDefault();
            blog.BlogOkunmaSayisi = blog.BlogOkunmaSayisi + 1;
            db.SaveChanges();

            return View();
        }
    }
}