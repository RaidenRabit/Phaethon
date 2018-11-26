using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Model;
using InternalApi.DataAccess;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.DataManagement
{
    internal class ElementManagement: IElementManagement
    {
        private readonly ElementDa _elementDa;

        internal ElementManagement()
        {
            _elementDa = new ElementDa();
        }

        public List<Element> GetInvoiceElements(int id)
        {
            using (var db = new DatabaseContext())
            {
                return _elementDa.GetInvoiceElements(db, id);
            }
        }
    }
}