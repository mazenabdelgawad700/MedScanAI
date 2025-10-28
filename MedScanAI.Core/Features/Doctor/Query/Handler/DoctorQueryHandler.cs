using AutoMapper;
using MediatR;
using MedScanAI.Core.Features.Doctor.Query.Model;
using MedScanAI.Core.Features.Doctor.Query.Response;
using MedScanAI.Service.Abstracts;
using MedScanAI.Shared.Base;

namespace MedScanAI.Core.Features.Doctor.Query.Handler
{
    public class DoctorQueryHandler : IRequestHandler<GetAllDoctorsQuery, ReturnBase<IQueryable<GetAllDoctorsResponse>>>,
        IRequestHandler<GetAllActiveDoctorsQuery, ReturnBase<IQueryable<GetAllActiveDoctorsResponse>>>,
        IRequestHandler<GetAllDoctorsCountQuery, ReturnBase<int>>,
        IRequestHandler<GetActiveDoctorsCountQuery, ReturnBase<int>>
    {

        private readonly IDoctorService _doctorService;
        private readonly IMapper _mapper;

        public DoctorQueryHandler(IDoctorService doctorService, IMapper mapper)
        {
            _doctorService = doctorService;
            _mapper = mapper;
        }

        public async Task<ReturnBase<IQueryable<GetAllDoctorsResponse>>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var doctors = await _doctorService.GetAllDoctorsAsync();

                if (!doctors.Succeeded)
                    return ReturnBaseHandler.Failed<IQueryable<GetAllDoctorsResponse>>(doctors.Message ?? "Failed to retrieve doctors.");

                var doctorResponses = _mapper
                                     .Map<List<GetAllDoctorsResponse>>(doctors.Data)
                                     .AsQueryable();

                return ReturnBaseHandler.Success(doctorResponses);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<IQueryable<GetAllDoctorsResponse>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ReturnBase<IQueryable<GetAllActiveDoctorsResponse>>> Handle(GetAllActiveDoctorsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var doctors = await _doctorService.GetActiveDoctorsAsync();

                if (!doctors.Succeeded)
                    return ReturnBaseHandler.Failed<IQueryable<GetAllActiveDoctorsResponse>>(doctors.Message ?? "Failed to retrieve active doctors.");

                var doctorResponses = _mapper
                                    .Map<List<GetAllActiveDoctorsResponse>>(doctors.Data)
                                    .AsQueryable();

                return ReturnBaseHandler.Success(doctorResponses);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<IQueryable<GetAllActiveDoctorsResponse>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ReturnBase<int>> Handle(GetAllDoctorsCountQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var countResult = await _doctorService.GetAllDoctorsCountAsync();
                if (!countResult.Succeeded)
                    return ReturnBaseHandler.Failed<int>(countResult.Message ?? "Failed to retrieve doctors count.");
                return ReturnBaseHandler.Success(countResult.Data);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<int>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<int>> Handle(GetActiveDoctorsCountQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var countResult = await _doctorService.GetActiveDoctorsCountAsync();
                if (!countResult.Succeeded)
                    return ReturnBaseHandler.Failed<int>(countResult.Message ?? "Failed to retrieve active doctors count.");
                return ReturnBaseHandler.Success(countResult.Data);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<int>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
