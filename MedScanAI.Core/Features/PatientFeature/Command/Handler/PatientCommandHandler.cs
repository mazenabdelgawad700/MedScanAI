using AutoMapper;
using MediatR;
using MedScanAI.Core.Features.PatientFeature.Command.Model;
using MedScanAI.Domain.Entities;
using MedScanAI.Service.Abstracts;
using MedScanAI.Shared.Base;

namespace MedScanAI.Core.Features.PatientFeature.Command.Handler
{
    public class PatientCommandHandler :
        IRequestHandler<CreatePatientProfileCommand, ReturnBase<bool>>,
        IRequestHandler<UpdatePatientProfileCommand, ReturnBase<bool>>,
        IRequestHandler<UpdatePatientAllergyCommand, ReturnBase<bool>>,
        IRequestHandler<UpdatePatientChronicDiseaseCommand, ReturnBase<bool>>,
        IRequestHandler<UpdatePatientCurrentMedicationCommand, ReturnBase<bool>>,
        IRequestHandler<DeletePatientAllergyCommand, ReturnBase<bool>>,
        IRequestHandler<DeletePatientChronicDiseaseCommand, ReturnBase<bool>>,
        IRequestHandler<DeletePatientCurrentMedicationCommand, ReturnBase<bool>>
    {
        private readonly IPatientProfileService _patientProfileService;
        private readonly IMapper _mapper;

        public PatientCommandHandler(IPatientProfileService patientProfileService, IMapper mapper)
        {
            _patientProfileService = patientProfileService;
            _mapper = mapper;
        }


        public async Task<ReturnBase<bool>> Handle(CreatePatientProfileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _patientProfileService.CreatePatientProfileAsync(
                    request.ChronicDiseases,
                    request.CurrentMedication,
                    request.Allergies,
                    request.PatientId
                );

                if (!result.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(result.Message);


                return ReturnBaseHandler.Success(true, result.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ReturnBase<bool>> Handle(UpdatePatientProfileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var mappedResult = _mapper.Map<Patient>(request);
                var updateProfileResult = await _patientProfileService.UpdatePatientProfileAsync(mappedResult);
                if (!updateProfileResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(updateProfileResult.Message);

                return ReturnBaseHandler.Success(true, updateProfileResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ReturnBase<bool>> Handle(UpdatePatientAllergyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var allergies = _mapper.Map<PatientAllergy>(request.Allergy);
                var updateAllergyResult = await _patientProfileService.UpdatePatientAllergyAsync(allergies);
                if (!updateAllergyResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(updateAllergyResult.Message);
                return ReturnBaseHandler.Success(true, updateAllergyResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ReturnBase<bool>> Handle(UpdatePatientChronicDiseaseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var chronicDisease = _mapper.Map<PatientChronicDisease>(request.ChronicDisease);
                var updateChronicDiseaseResult = await _patientProfileService.UpdatePatientChronicDiseaseAsync(chronicDisease);
                if (!updateChronicDiseaseResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(updateChronicDiseaseResult.Message);
                return ReturnBaseHandler.Success(true, updateChronicDiseaseResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ReturnBase<bool>> Handle(UpdatePatientCurrentMedicationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentMedication = _mapper.Map<PatientCurrentMedication>(request.CurrentMedication);
                var updateCurrentMedicationResult = await _patientProfileService.UpdatePatientCurrentMedicationAsync(currentMedication);
                if (!updateCurrentMedicationResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(updateCurrentMedicationResult.Message);
                return ReturnBaseHandler.Success(true, updateCurrentMedicationResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ReturnBase<bool>> Handle(DeletePatientAllergyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteResult = await _patientProfileService.DeletePatientAllergyAsync(request.Id);
                if (!deleteResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(deleteResult.Message);
                return ReturnBaseHandler.Success(true, deleteResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ReturnBase<bool>> Handle(DeletePatientChronicDiseaseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteResult = await _patientProfileService.DeletePatientChronicDiseaseAsync(request.Id);
                if (!deleteResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(deleteResult.Message);
                return ReturnBaseHandler.Success(true, deleteResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ReturnBase<bool>> Handle(DeletePatientCurrentMedicationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteResult = await _patientProfileService.DeletePatientCurrentMedicationAsync(request.Id);
                if (!deleteResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(deleteResult.Message);
                return ReturnBaseHandler.Success(true, deleteResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
