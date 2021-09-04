using CLUNL.Data.Serializables.SSS;
using Site13Kernel.GameLogic.CampaignActions;
using System;
using System.Collections;
using System.Collections.Generic;
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
            var Data=Deserializer.Deserialize<CampaignAction>(contents);
            SerialCampaignScript campaignActions = new SerialCampaignScript();
            campaignActions.CoreData=Data;
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
    }
    public interface ISerialData<T>
    {
        int Position { get; set; }
        T NextData();
        int GetPosition();
        void SetPosition(int position);
    }
}
