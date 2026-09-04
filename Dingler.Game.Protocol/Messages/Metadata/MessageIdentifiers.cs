namespace Dingler.Game.Protocol.Messages.Metadata
{
    public static class MessageIdentifiers
    {
        public static class Session
        {
            // Session Requests
            public const int NEW_SESSION_REQUEST = 0;
            public const int AUTHENTICATION_REQUEST = 1;
            public const int PING_REQUEST = 2;
        }

        public static class Chat
        {
            public const int CHAT = 3;
        }

        public static class LoadBalancer
        {
            public const int RECONNECT_TO_DISCONNECTED_GAME = 22013;
            public const int READY_FOR_GAME = 22027;
            public const int READY_FOR_GAME_EVENTS = 22029;
        }

        public static class Profile
        {
            public const int GET_DECK_INFO = 2083;
            public const int ADD_NEW_DECK = 2089;
            public const int REMOVE_DECK = 2093;
            public const int UPDATE_DECK = 2095;
            public const int STREAM_PROFILE_INFO = 2210;
            public const int LOGIN_COMPLETE = 2211;
        }
        public static class Friends
        {
            public const int ADD_FRIEND_REQUEST = 2149;
            public const int ACCEPT_FRIEND_REQUEST = 2157;
            public const int FRIEND_ADDED_EVENT = 2194;
            public const int PENDING_FRIEND_REQUESTS_ARRIVED = 2199;
            public const int FRIEND_REQUEST_REMOVED = 2200;
            public const int FRIEND_LIST_ARRIVED = 2202;
            public const int FRIEND_COMES_ONLINE = 2203;
            public const int FRIEND_GOES_OFFLINE = 2204;
            public const int FRIEND_REQUEST_RECEIVED = 2192;
            public const int FRIEND_REQUEST_ACCEPTED = 2193;
        }

        public static class Challenge
        {
            public const int SEND_DIRECT_CHALLENGE_RESPONSE = 4013;
            public const int CHALLENGE_REQUEST_RESPONSE = 4017;
            public const int DIRECT_CHALLENGE_RECEIVED = 4027;
            public const int SEND_CHALLENGE_SESSION = 4028;
        }
        public static class Mail
        {
            public const int GET_UNREAD_MAIL_RESPONSE = 60007;
        }

        public static class Game
        {
            public const int PLAYER_TRANSACTION = 3029;
            public const int SESSION_SYNC_EVENT = 3055;
        }

        public static class Tournaments
        {
            public const int REQUEST_ENTRY = 25027;
            public const int ENTER_TOURNAMENT = 25029;
            public const int INITIALIZE = 25054;
            public const int TOURNAMENT_INFO = 25059;
            public const int TOURNAMENT_SESSION_START = 25060;
            public const int GO_TO_LOBBY = 25062;
            public const int GO_TO_DECK_CONSTRUCTION = 25072;
        }
    }
}
