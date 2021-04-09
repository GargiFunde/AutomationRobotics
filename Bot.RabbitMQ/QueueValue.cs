using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.RabbitMQ
{
    public class QueueValue
    {
        public int MessagePriority {get;set; }

        public Dictionary<string, object> Items { get; set; }
      
        

        public object GetValueByKey(string key) {
            Object obj = null;

            //can we use Linq in that matter ?
            if (Items.ContainsKey(key))
            {
                Items.TryGetValue(key, out obj);
            }
            else
            {

                throw new KeyNotFoundException();
                
            }

            return obj;
    }

        public int GetCount() {
           
            return Items.Count();
           
        }



        public string GetMessagePriority() {
            string result = null;
            switch (MessagePriority)
            {
                case 1:
                    result = "Low";
                break;

                case 2:
                    result = "Normal";
                    break;
                case 3:
                    result = "High";
                    break;

                
            }
            return result;
        
        }

    }
}
