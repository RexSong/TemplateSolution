using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TemplateSolution.Repository.Domain;
using TemplateSolution.Repository.Interface;

namespace TemplateSolution.App
{
    public class RawDataManager: BaseManager<RawData>
    {
        public void Add(RawData rawData)
        {
            Repository.Add(rawData);
        }

        public void Update(RawData rawData)
        {
            Repository.Update(rawData);
        }


        public List<RawData> GetList()
        {
            var rawDatas = UnitWork.Find<RawData>(null);

            return rawDatas.ToList();
        }

        public RawDataManager(IUnitWork unitWork, IRepository<RawData> repository) : base(unitWork, repository)
        {
        }
    }
}
