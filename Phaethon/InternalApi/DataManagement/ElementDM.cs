using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Model;
using InternalApi.DataAccess;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.DataManagement
{
    internal class ElementDM: IElementDM
    {
        private readonly ElementDa _elementDa;

        internal ElementDM()
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