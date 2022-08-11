using System;
using System.Collections.Generic;

namespace Site13Project.Core
{
    [Serializable]
    public class Project
    {
        public List<Configuration> Configurations = new List<Configuration>();
        public Configuration ObtainCurrent(params string[] conditions)
        {
            Configuration conf = new Configuration();
            foreach (var item in Configurations)
            {
                if (item.CheckCondition(conditions))
                {
                    {
                        conf.Combine(item);
                    }
                }
            }
            return conf;
        }
    }
}
