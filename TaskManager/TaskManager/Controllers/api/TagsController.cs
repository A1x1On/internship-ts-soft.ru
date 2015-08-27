using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaskManager.Realizations;
using WebMatrix.WebData;

namespace TaskManager.Controllers.api
{
    [RoutePrefix("api/Tags")]
    public class TagsController : ApiController
    {
        /// <summary>
        /// Instance RealizeManager : IManager is
        /// </summary>
        IManager m_Realize = new RealizeManager();

        /// <summary>
        /// Getting list of Tags with inputted Keyword [AJAX]
        /// </summary>
        /// <param name="name">Key word from (text input of class="inputTag")</param>
        /// <returns>list of received tags</returns>
        [Route("GetTags")]
        public IEnumerable<Tags> GetTags(string name)
        {
            IEnumerable<Tags> tags = m_Realize.GetTags(name);

            return tags;
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}