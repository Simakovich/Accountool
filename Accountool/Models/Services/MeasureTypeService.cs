//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Accountool.Models.Entities;
//using Accountool.Models.DataAccess;
//using Accountool.Models.ViewModel;

//namespace Accountool.Models.Services
//{
//    public interface IMeasurementService
//    {
//        Task<IEnumerable<Indication>> GetAll();
//        Task<Indication> GetById(int id);
//        Task Add(Indication entity);
//        Task Update(Indication entity);
//        Task Remove(int id);
//        Task<IEnumerable<Indication>> GetForSchetchik(int SchetchikId);
//        Task<IEnumerable<Indication>> GetByMeasureId();
//    }

//    public class MeasurementService : IMeasurementService
//    {
//        private readonly IRepository<Indication> _indications;
//        private readonly IRepository<Schetchik> _schetchiks;
//        private readonly IRepository<MeasureType> _measureTypes;

//        public MeasurementService(
//             IRepository<Indication> indications,
//             IRepository<Schetchik> schetchiks,
//             IRepository<MeasureType> measureTypes)
//        {
//            _indications = indications;
//            _schetchiks = schetchiks;
//            _measureTypes = measureTypes;
//        }

//        public async Task<IEnumerable<Indication>> GetAll()
//        {
//            return _indications.GetAll();
//        }


//        public async Task<IEnumerable<Indication>> GetByMeasureId()
//        {
//            var indications = (from mt in _measureTypes.GetAll()
//                               join s in _schetchiks.GetAll() on mt.Id equals s.MeasureTypeId
//                               join i in _indications.GetAll() on s.Id equals i.SchetchikId
//                               where mt.Id == 1
//                               select i).ToList();
//            return indications;
//        }

//        public async Task<Indication> GetById(int id)
//        {
//            return await _indications.GetByIdAsync(id);
//        }

//        public async Task Add(Indication entity)
//        {
//            await _indications.AddAsync(entity);
//        }

//        public async Task Update(Indication entity)
//        {
//            _indications.Update(entity);
//        }

//        public async Task Remove(int id)
//        {
//            var model = await _indications.GetByIdAsync(id);
//            _indications.Remove(model);
//        }

//        public async Task<IEnumerable<Indication>> GetForSchetchik(int SchetchikId)
//        {
//            return _indications.GetAll().Where(i => i.SchetchikId == SchetchikId).AsEnumerable();
//        }
//    }
//}