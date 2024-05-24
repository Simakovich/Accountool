using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accountool.Models.Entities;
using Accountool.Models.DataAccess;
using Accountool.Models.ViewModel;
using System.Data;
using Accountool.Models.Models;
using Humanizer;
using System;

namespace Accountool.Models.Services
{
    public interface IMeasurementService
    {
        Task<Indication> GetById(int id);
        Task Add(Indication entity);
        Task Update(Indication entity);
        Task Remove(int id);
        Task<IEnumerable<Indication>> GetForSchetchik(int SchetchikId);
        Task<IQueryable<FullIndicationModel>> GetFilteredMeasures(int measureTypeid, int? townId = null, int? placeId = null, int? monthFrom = null, int? monthTo = null, int? yearFrom = null, int? yearTo = null);
        Task<FullIndicationModel> GetLastMeasureByCounter(int measureTypeid, int counterId);
        Task<IEnumerable<MeasureType>> GetAllMeasureTypes();
        Task<IEnumerable<Indication>> GetAllIndications();
        Task<FirstLastYearModel> GetMinMaxYear(int measureTypeid);
        Task<IEnumerable<IdNameModel>> GetPlaces(int measureTypeid);
        Task<IEnumerable<IdNameModel>> GetTowns(int measureTypeid);
    }

    public class MeasurementService : IMeasurementService
    {
        private readonly IRepository<Indication> _indications;
        private readonly IRepository<Schetchik> _schetchiks;
        private readonly IRepository<Place> _places;
        private readonly IRepository<Town> _town;
        private readonly IRepository<MeasureType> _measureTypes;

        public MeasurementService(
             IRepository<Indication> indications,
             IRepository<Schetchik> schetchiks,
             IRepository<Place> place,
             IRepository<Town> town,
        IRepository<MeasureType> measureTypes)
        {
            _indications = indications;
            _schetchiks = schetchiks;
            _measureTypes = measureTypes;
            _places = place;
            _town = town;
        }

        public async Task<IEnumerable<Indication>> GetAllIndications()
        {
            return _indications.GetAll();
        }

        public async Task<IEnumerable<MeasureType>> GetAllMeasureTypes()
        {
            return _measureTypes.GetAll();
        }

        public async Task<IQueryable<FullIndicationModel>> GetFilteredMeasures(
            int measureTypeid,
            int? townId = null,
            int? placeId = null,
            int? monthFrom = null,
            int? monthTo = null,
            int? yearFrom = null,
            int? yearTo = null)
        {
            var indications = from mt in _measureTypes.GetAll()
                              join s in _schetchiks.GetAll() on mt.Id equals s.MeasureTypeId
                              join i in _indications.GetAll() on s.Id equals i.SchetchikId
                              join k in _places.GetAll() on s.PlaceId equals k.Id
                              join t in _town.GetAll() on k.TownId equals t.Id
                              where mt.Id == measureTypeid
                              && (placeId == null || k.Id == placeId)
                              && (townId == null || k.TownId == townId)
                              select new { i, k, t, mt, s  };

            if (monthFrom != null)
            {
                indications = indications.Where(x => x.i.Month.Month >= monthFrom);
            }

            if (monthTo != null)
            {
                indications = indications.Where(x => x.i.Month.Month <= monthTo);
            }

            if (yearFrom != null)
            {
                indications = indications.Where(x => x.i.Month.Year >= yearFrom);
            }

            if (yearTo != null)
            {
                indications = indications.Where(x => x.i.Month.Year <= yearTo);
            }

            return indications.Select(x => new FullIndicationModel()
            {
                PlaceId = x.k.Id,
                PlaceName = x.k.Name,
                Address = x.k.Address,
                TownName = x.t.Name,
                Indication = x.i,
                MeasureTypeId = x.mt.Id
            });
        }

        public async Task<FullIndicationModel> GetLastMeasureByCounter(
            int measureTypeid,
            int counterId)
        {
            var indications = from mt in _measureTypes.GetAll()
                              join s in _schetchiks.GetAll() on mt.Id equals s.MeasureTypeId
                              join i in _indications.GetAll() on s.Id equals i.SchetchikId
                              join k in _places.GetAll() on s.PlaceId equals k.Id
                              join t in _town.GetAll() on k.TownId equals t.Id
                              where mt.Id == measureTypeid
                              && s.Id == counterId
                              select new { i, k, t };


            var indication = indications.OrderBy(x => x.i.Month).FirstOrDefault();


            var result = new FullIndicationModel()
            {
                PlaceId = indication.k.Id,
                PlaceName = indication.k.Name,
                Address = indication.k.Address,
                TownName = indication.t.Name,
                Indication = indication.i
            };

            return result;
        }

        public async Task<FirstLastYearModel> GetMinMaxYear(
            int measureTypeid)
        {
            var minMaxDate = from mt in _measureTypes.GetAll()
                              join s in _schetchiks.GetAll() on mt.Id equals s.MeasureTypeId
                              join i in _indications.GetAll() on s.Id equals i.SchetchikId
                              where mt.Id == measureTypeid
                              select i.Month;
            if (minMaxDate != null && minMaxDate.Any())
            {
                var minMaxDateTime = new FirstLastYearModel
                {
                    FirstDate = minMaxDate.Min(),
                    LastDate = minMaxDate.Max()
                };
                return minMaxDateTime;
            }

            return null;
        }

        public async Task<IEnumerable<IdNameModel>> GetPlaces(
            int measureTypeid)
        {
            var places = from mt in _measureTypes.GetAll()
                         join s in _schetchiks.GetAll() on mt.Id equals s.MeasureTypeId
                         join i in _indications.GetAll() on s.Id equals i.SchetchikId
                         join k in _places.GetAll() on s.PlaceId equals k.Id
                         where mt.Id == measureTypeid
                         select new IdNameModel { Id = k.Id, Name = k.Name };

            return places;
        }

        public async Task<IEnumerable<IdNameModel>> GetTowns(
            int measureTypeid)
        {
            var townsId = from mt in _measureTypes.GetAll()
                         join s in _schetchiks.GetAll() on mt.Id equals s.MeasureTypeId
                         join i in _indications.GetAll() on s.Id equals i.SchetchikId
                         join k in _places.GetAll() on s.PlaceId equals k.Id
                         join t in _town.GetAll() on k.TownId equals t.Id
                         where mt.Id == measureTypeid
                         select new IdNameModel { Id = t.Id, Name = t.Name };

            return townsId;
        }

        public async Task<Indication> GetById(int id)
        {
            return await _indications.GetByIdAsync(id);
        }

        public async Task Add(Indication entity)
        {
            await _indications.AddAsync(entity);
        }

        public async Task Update(Indication entity)
        {
            _indications.Update(entity);
        }

        public async Task Remove(int id)
        {
            var model = await _indications.GetByIdAsync(id);
            _indications.Remove(model);
        }

        public async Task<IEnumerable<Indication>> GetForSchetchik(int SchetchikId)
        {
            return _indications.GetAll().Where(i => i.SchetchikId == SchetchikId).AsEnumerable();
        }
    }
}