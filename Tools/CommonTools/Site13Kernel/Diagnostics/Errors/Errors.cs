using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Diagnostics.Errors
{
    public class UndefinedCampaignID: ISite13Error
    {
        int ______;
        public UndefinedCampaignID(int ID)
        {
            ______ = ID;
        }
        public override string ToString()
        {
            return $"Undefined Campaign ID: {______}";
        }
    }
    public interface ISite13Error
    {

    }
}
