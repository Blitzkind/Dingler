namespace Dingler.Game.Protocol.Messages.Metadata
{
    public static class IssuerIdentifiers
    {
        public static class Session
        {
            public const string SESSION = "Session";
        }
        public static class Mail
        {
            public const string UNREAD_MAIL_RESPONSE = "100.100.100.100.100.100";
        }

        public static class Profile
        {
            public const string STREAM_PROFILE_INFO = "200.200.200.200.200.200";
            public const string LOGIN_COMPLETE = "201.201.201.201.201.201";
            public const string ADD_NEW_DECK = "202.202.202.202.202.202";
            public const string GET_DECK_INFO = "203.203.203.203.203.203";
            public const string UPDATE_DECK = "204.204.204.204.204.204";
            public const string REMOVE_DECK = "205.205.205.205.205.205";
        }

        public static class Friends
        {
            public const string GET_PENDING_FRIEND_REQUESTS = "300.300.300.300.300.300";
            public const string GET_MUTUAL_FRIENDS = "301.301.301.301.301.301";
            public const string ACCEPT_FRIEND_REQUEST = "302.302.302.302.302.302";
            public const string REMOVE_FRIEND_REQUEST = "303.303.303.303.303.303";
            public const string FRIEND_COMES_ONLINE = "304.304.304.304.304.304";
            public const string FRIEND_GOES_OFFLINE = "305.305.305.305.305.305";
            public const string FRIEND_REQUEST_RECEIVED = "306.306.306.306.306.306";
            public const string FRIEND_REQUEST_ACCEPTED = "307.307.307.307.307.307";
            public const string ADD_FRIEND_REQUEST = "308.308.308.308.308.308";
            public const string FRIEND_ADDED_EVENT = "309.309.309.309.309.309";
            public const string FRIEND_REQUEST_REMOVED = "310.310.310.310.310.310";
        }

        public static class Challenge
        {
            public const string DIRECT_CHALLENGE_RECEIVED = "400.400.400.400.400.400";
            public const string SEND_DIRECT_CHALLENGE_RESPONSE = "401.401.401.401.401.401";
            public const string CHALLENGE_REQUEST_RESPONSE = "402.402.402.402.402.402";
            public const string SEND_CHALLENGE_SESSION = "403.403.403.403.403.403";
            public const string SESSION_SYNC_EVENT = "404.404.404.404.404.404";
        }

        public static class LoadBalancer
        {
            public const string RECONNECT_TO_DISCONNECTED_GAME = "500.500.500.500.500.500";
            public const string READY_FOR_GAME = "501.501.501.501.501.501";
            public const string READY_FOR_GAME_EVENTS = "502.502.502.502.502.502";
        }

        public static class Game
        {
            public const string PLAYER_TRANSACTION = "600.600.600.600.600.600";
            public const string SESSION_SYNC_EVENT = "601.601.601.601.601.601";
        }

        public static class Tournaments
        {
            public const string INITIALIZE = "700.700.700.700.700.700";
            public const string REQUEST_ENTRY = "701.701.701.701.701.701";
            public const string ENTER_TOURNAMENT = "702.702.702.702.702.702";
            public const string TOURNAMENT_SESSION_START = "703.703.703.703.703.703";
            public const string TOURNAMENT_INFO = "704.704.704.704.704.704";
            public const string GO_TO_LOBBY = "705.705.705.705.705.705";
            public const string GO_TO_DECK_CONSTRUCTION = "706.706.706.706.706.706";
        }
    }
}
