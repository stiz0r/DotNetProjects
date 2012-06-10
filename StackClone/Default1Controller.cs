using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Extensions;
using Raven.Client.Indexes;
using StackClone.Models;

namespace StackClone
{
    public class Default1Controller : Controller
    {
        //
        // GET: /Default1/

        private DocumentStore _document_store;
        public DocumentStore DocumentStore
        {
            get
            {
                if(_document_store == null)
                    _document_store = HttpContext.Application.Get("DocumentStore") as DocumentStore;
                return _document_store;
            }
        }

        public ActionResult Index()
        {
            List<Question> questions = new List<Question>();
            using (IDocumentSession session = DocumentStore.OpenSession())
            {
                questions.AddRange(session.Query<Question>().ToList());
            }

            
            return Json(questions, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ById(string id)
        {
            Question question = null;
            using (IDocumentSession session = DocumentStore.OpenSession())
            {
                question = session.Load<Question>(id);
            }


            return Json(question, JsonRequestBehavior.AllowGet);   
        }

        public ActionResult BySubject(string subject)
        {
            List<Question> questions = new List<Question>();
            using (IDocumentSession session = DocumentStore.OpenSession())
            {
                questions.AddRange(session.Query<Question>().Where(q => q.Subject == subject).ToList());
            }



            return Json(questions, JsonRequestBehavior.AllowGet);     
        }

        public ActionResult EditQuestion1()
        {
            Question question = null;
            using (IDocumentSession session = DocumentStore.OpenSession())
            {
                question = session.Load<Question>("questions/1");
            }

            
            UpdateModel(question);

            return Json(question, JsonRequestBehavior.AllowGet);
        }
    }


    public class Question_ByDate : AbstractIndexCreationTask<Question>
    {
        public Question_ByDate()
        {
            Map = docs => from doc in docs select new {doc.Date};
        }
    }

    public class User_ByUserName : AbstractIndexCreationTask<User>
    {
        public User_ByUserName()
        {
            Map = docs => from doc in docs select new {doc.UserName};
        }
    }

}
