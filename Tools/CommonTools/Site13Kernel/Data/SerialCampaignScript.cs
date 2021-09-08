using CLUNL.Data.Serializables.SSS;
using Site13Kernel.GameLogic.CampaignActions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Site13Kernel.Data
{
    [Serializable]
    public class SerialCampaignScript : IEnumerable<CampaignAction>, ISerialData<CampaignAction>
    {
        /// <summary>
        /// Main Data
        /// </summary>
        public List<CampaignAction> CoreData;
        int Index = 0;
        public int Position
        {
            get => Index;
            set => Index = value;
        }
        public IEnumerator<CampaignAction> GetEnumerator()
        {
            foreach (var item in CoreData)
            {
                Index++;
                yield return item;
            }
        }
        public List<string> Serialize()
        {
            return Serializer.Serialize(CoreData);
        }
        public static SerialCampaignScript Deserialize(List<string> contents)
        {
            var Data = Deserializer.Deserialize<CampaignAction>(contents);
            SerialCampaignScript campaignActions = new SerialCampaignScript();
            campaignActions.CoreData = Data;
            return campaignActions;
        }
        public static SerialCampaignScript Deserialize(string contents)
        {
            List<string> _Data = new List<string>();
            SerialCampaignScript campaignActions = new SerialCampaignScript();
            using (var SR = new StringReader(contents))
            {
                campaignActions.CoreData = new List<CampaignAction>();
                Deserializer.Deserialize(SR, ref campaignActions.CoreData);
                //string L;
                //while ((L=SR.ReadLine())!=null)
                //{
                //    _Data.Add(L);
                //}
            }
            //var Data = Deserializer.Deserialize<CampaignAction>(_Data);
            //campaignActions.CoreData = Data;
            return campaignActions;
        }
        public int GetPosition()
        {
            return Index;
        }

        public CampaignAction NextData()
        {
            Index++;
            if (Index < CoreData.Count)
            {
                return CoreData[Index];
            }
            return null;
        }

        public void SetPosition(int position)
        {
            Index = position;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return CoreData.GetEnumerator();
        }

        public CampaignAction CurrentData()
        {
            return CoreData[Index];
        }

        public bool MoveNext()
        {

            Index++;
            if (Index < CoreData.Count)
                return true;
            Index = CoreData.Count - 1;
            return false;
        }
    }
    public interface ISerialData<T>
    {
        int Position { get; set; }
        T NextData();
        bool MoveNext();
        T CurrentData();
        int GetPosition();
        void SetPosition(int position);
    }
}
