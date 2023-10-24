using Microsoft.AspNetCore.Authorization;
using StudyRoomRental.BusinessTier.Enums;
using StudyRoomRental.BusinessTier.Utils;

namespace StudyRoomRental.BusinessTier.Validators;

public class CustomAuthorizeAttribute : AuthorizeAttribute
{
	public CustomAuthorizeAttribute(params RoleEnum[] roleEnums)
	{
		var allowedRolesAsString = roleEnums.Select(x => x.GetDescriptionFromEnum());
		Roles = string.Join(",", allowedRolesAsString);
	}
}