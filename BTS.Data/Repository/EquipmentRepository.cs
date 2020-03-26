using BTS.Common;
using BTS.Data.Infrastructure;
using BTS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Data.Repository
{
    public interface IEquipmentRepository : IRepository<Equipment>
    {
        bool IsUsed(string Id);
        IEnumerable<EquipmentTab> GetEquipmentTabs();
    }

    public class EquipmentRepository : RepositoryBase<Equipment>, IEquipmentRepository
    {
        public EquipmentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<EquipmentTab> GetEquipmentTabs()
        {
            IQueryable<EquipmentTab> query = from equ in DbContext.Equipments
                                             group equ by new { equ.Name, equ.Band, equ.Tx } into equGroup
                                             select new EquipmentTab
                                             {
                                                 Name = equGroup.Key.Name,
                                                 Band = equGroup.Key.Band,
                                                 Tx = equGroup.Key.Tx,
                                                 MOBIFONE = (from mobi in DbContext.Equipments
                                                                             where mobi.OperatorRootID == CommonConstants.Sheet_Equipment_MobiFone && mobi.Name == equGroup.Key.Name && mobi.Band == equGroup.Key.Band && mobi.Tx == equGroup.Key.Tx
                                                                             select mobi.MaxPower).FirstOrDefault(),
                                                 VINAPHONE = (from vina in DbContext.Equipments
                                                                              where vina.OperatorRootID == CommonConstants.Sheet_Equipment_VinaPhone && vina.Name == equGroup.Key.Name && vina.Band == equGroup.Key.Band && vina.Tx == equGroup.Key.Tx
                                                                              select vina.MaxPower).FirstOrDefault(),
                                                 VIETTEL = (from viettel in DbContext.Equipments
                                                                            where viettel.OperatorRootID == CommonConstants.Sheet_Equipment_Viettel && viettel.Name == equGroup.Key.Name && viettel.Band == equGroup.Key.Band && viettel.Tx == equGroup.Key.Tx
                                                                            select viettel.MaxPower).FirstOrDefault(),
                                                 VNMOBILE = (from vnmobi in DbContext.Equipments
                                                                             where vnmobi.OperatorRootID == CommonConstants.Sheet_Equipment_VNMobile && vnmobi.Name == equGroup.Key.Name && vnmobi.Band == equGroup.Key.Band && vnmobi.Tx == equGroup.Key.Tx
                                                                             select vnmobi.MaxPower).FirstOrDefault(),
                                                 GTEL = (from gtel in DbContext.Equipments
                                                                         where gtel.OperatorRootID == CommonConstants.Sheet_Equipment_Gtel && gtel.Name == equGroup.Key.Name && gtel.Band == equGroup.Key.Band && gtel.Tx == equGroup.Key.Tx
                                                                         select gtel.MaxPower).FirstOrDefault()
                                             };
            return query;
        }

        public bool IsUsed(string Id)
        {
            //var query1 = from item in DbContext.Equipments
            //             where item.Id == Id
            //             select item.Id;
            //if (query1.Count() > 0) return true;

            return false;
        }
    }
}