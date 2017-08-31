using System;
using System.Collections.Generic;
using YAXLib;

namespace MoneyModels
{
    public class Owner
    {
        [YAXAttributeForClass]
        public string ownerId { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string quantity { get; set; }

        [YAXAttributeFor("quantity")]
        public DateTime asofDate { get; set; }
    }

    public class OwnershipDetails
    {
        public Int64 floatShares { get; set; }

        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement, EachElementName = "Owner")]
        public List<Owner> OwnerList { get; set; }

        private Dictionary<string, Owner> m_owner_db = null;
        public Dictionary<string, Owner> Owners
        {
            get
            {
                if (m_owner_db == null)
                {
                    m_owner_db = new Dictionary<string, Owner>();
                    for (int i = 0; i < OwnerList.Count; i++)
                    {
                        var data = OwnerList[i];
                        //m_owner_db[data.ownerId] = data;
                    }
                }
                return m_owner_db;
            }
        }
    }
}
