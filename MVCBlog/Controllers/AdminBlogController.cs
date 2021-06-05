using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBlog.Models;
using System.Web.Helpers;
using System.IO;
using System.Text.RegularExpressions;

namespace MVCBlog.Controllers
{
    public class AdminBlogController : Controller
    {
        DBBlogEntities db = new DBBlogEntities();
        // GET: AdminBlog
        public ActionResult Index()
        {
            var blogs = db.Blogs.ToList();
            return View(blogs);
        }

        // GET: AdminBlog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);

            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // GET: AdminBlog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminBlog/Create
        [HttpPost]
        public ActionResult Create(Blog blog, HttpPostedFileBase foto)
        {
            try
            {
                if (foto != null)
                {
                    WebImage webImage = new WebImage(foto.InputStream);
                    FileInfo fileInfo = new FileInfo(foto.FileName);
                    string newFoto = Guid.NewGuid().ToString() + fileInfo.Extension;
                    webImage.Resize(800, 350);
                    webImage.Save("~/Uploads/" + newFoto);
                    blog.Foto = "/Uploads/" + newFoto;

                }
                blog.BlogOkunmaSayisi = 0;
                blog.BlogTarih = DateTime.Now;
                db.Blogs.Add(blog);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminBlog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var blog = db.Blogs.Find(id);
            return View(blog);
        }

        // POST: AdminBlog/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection, HttpPostedFileBase foto, Blog blog)
        {
            try
            {
                var blogUpdate = db.Blogs.Find(id);

                if (foto != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(blog.Foto)))
                    {
                        System.IO.File.Delete(Server.MapPath(blog.Foto));
                    }

                    WebImage webImage = new WebImage(foto.InputStream);
                    FileInfo fileInfo = new FileInfo(foto.FileName);
                    string newFoto = Guid.NewGuid().ToString() + fileInfo.Extension;
                    webImage.Resize(800, 350);
                    webImage.Save("~/Uploads/" + newFoto);
                    blogUpdate.Foto = "/Uploads/" + newFoto;

                    blogUpdate.BlogBaslik = blog.BlogBaslik;
                    blogUpdate.BlogIcerik = blog.BlogIcerik;
                    blogUpdate.BlogOkunmaSayisi = blog.BlogOkunmaSayisi;
                    blogUpdate.BlogOkunmaSuresi = blog.BlogOkunmaSuresi;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                } else
                {
                    blogUpdate.BlogBaslik = blog.BlogBaslik;
                    blogUpdate.BlogIcerik = blog.BlogIcerik;
                    blogUpdate.BlogOkunmaSayisi = blog.BlogOkunmaSayisi;
                    blogUpdate.BlogOkunmaSuresi = blog.BlogOkunmaSuresi;
                    db.SaveChanges();
                }

                return View();

            }
            catch
            {
                return View();
            }
        }

        // GET: AdminBlog/Delete/5
        public ActionResult Delete(int id)
        {
            var blog = db.Blogs.Where(b => b.BlogID == id).SingleOrDefault();
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: AdminBlog/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var blog = db.Blogs.Where(b => b.BlogID == id).SingleOrDefault();
                if (blog == null)
                {
                    return HttpNotFound();
                }
                if (System.IO.File.Exists(Server.MapPath(blog.Foto)))
                {
                    System.IO.File.Delete(Server.MapPath(blog.Foto));
                }
                db.Blogs.Remove(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        protected string StripHtml(string Txt)
        {
            return Regex.Replace(Txt, "<(.|\\n)*?>", string.Empty);
        }
    }
}
