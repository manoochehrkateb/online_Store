using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Online_Store.Classes;
using Online_Store.Models;

namespace Online_Store.Api
{
     //  [Route("api/products/{postId:int}/Comments")]
      [RoutePrefix("api/products/{postId:int}/Comments")]
    public class CommentsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: api/Comments

        [HttpGet]
        public IQueryable<Comment> Gets(int postId)
        {
            return db.Comments.Where(x => x.ProductIdForComment == postId);
        }

        //GET api/values/5
        [Route("")]
        public string Get()
        {
            return "value";
        }

        //  POST api/values
        [Route("")]
        [HttpPost]
        public async Task<Comment> Post(int postId, [FromBody]Comment comment)
        {
           // Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = PersianDateExtensionMethods.GetPersianCulture();
            //DateTime dob = DateTime.Now;  
            comment.ProductIdForComment = postId;
            comment.Author = User.Identity.Name ?? "مهمان";
            comment.PostedDate = DateTime.Now.Date.ToString("d");
            
            db.Comments.Add(comment);
            await db.SaveChangesAsync();

            return comment;
        }

        // PUT: api/Comments/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Comments/5
        public void Delete(int id)
        {
        }
    }
}