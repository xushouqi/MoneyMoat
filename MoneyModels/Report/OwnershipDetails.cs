using System;
using System.Collections.Generic;
using YAXLib;

namespace MoneyModels
{
    public class Owner
    {
        [YAXAttributeForClass]
        public string ownerId { get; set; }
        public int type { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public string currency { get; set; }

        [YAXAttributeFor("quantity")]
        public DateTime asofDate { get; set; }
    }

    public class OwnershipDetails
    {
        public int floatShares { get; set; }


        //[YAXDictionary(EachPairName = "Owner", KeyName = "ownerId",
        //          SerializeKeyAs = YAXNodeTypes.Attribute, SerializeValueAs = YAXNodeTypes.Element)]
        //[YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
        //public Dictionary<string, Owner> Owners { get; set; }

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
                        m_owner_db[data.ownerId] = data;
                    }
                }
                return m_owner_db;
            }
        }
    }
}
