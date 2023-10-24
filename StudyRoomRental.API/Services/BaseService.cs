using StudyRoomRental.DataTier.Models;
using StudyRoomRental.DataTier.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Claims;


namespace StudyRoomRental.API.Services
{
    public abstract class BaseService<T> where T : class
    {
        protected IUnitOfWork<StudyRoomRentalContext> _unitOfWork;
        protected ILogger<T> _logger;

        public BaseService(IUnitOfWork<StudyRoomRentalContext> unitOfWork, ILogger<T> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;

        }

    }
}
