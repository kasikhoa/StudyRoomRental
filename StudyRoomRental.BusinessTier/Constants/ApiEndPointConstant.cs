﻿using System.Net.NetworkInformation;

namespace StudyRoomRental.BusinessTier.Constants;

public static class ApiEndPointConstant
{

    public const string RootEndPoint = "/api";
    public const string ApiVersion = "/v1";
    public const string ApiEndpoint = RootEndPoint + ApiVersion;

    public static class Authentication
    {
        public const string AuthenticationEndpoint = ApiEndpoint + "/auth";
        public const string Login = AuthenticationEndpoint + "/login";
    }
    public static class Account
    {
        public const string AccountsEndpoint = ApiEndpoint + "/accounts";
        public const string AccountEndpoint = AccountsEndpoint + "/{id}";
        public const string AccountUpdateEndpoint = AccountEndpoint + "/roles";
    }

    public static class RoomType
    {
        public const string RoomTypesEndPoint = ApiEndpoint + "/roomTypes";
        public const string RoomTypeEndPoint = RoomTypesEndPoint + "/{id}";
    }

    public static class Room
    {
        public const string RoomsEndPoint = ApiEndpoint + "/rooms";
        public const string RoomEndPoint = RoomsEndPoint + "/{id}";
    }

    public static class RoomSchedule
    {
        public const string RoomSchedulesEndPoint = ApiEndpoint + "/roomSchedules";
        public const string RoomScheduleEndPoint = RoomSchedulesEndPoint + "/{id}";
    }

    public static class Order
    {
        public const string OrdersEndPoint = ApiEndpoint + "/orders";
        public const string OrderEndPoint = OrdersEndPoint + "/{id}";
        public const string FeedbackEndPoint = OrderEndPoint + "/feedback";
        public const string FeedbacksEndPoint = OrdersEndPoint + "/feedback";
    }
}