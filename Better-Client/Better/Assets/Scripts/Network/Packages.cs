public enum ServerPackages {
    SWelcomeMsg = 1,
    SAccountExist,
    SLobbyCreated,
    SSendLobbyList,
    SSendUsersOnlineList,
    SPlayerJoined,
    SMatchOver,
    SGetBalance,
    SNoUpdate,
    SUpdate,
}

public enum ClientPackages {
    CThankYouMsg = 1,
    CCheckForAccount,
    CCreateAccount,
    CGameOver,
    CCreateLobby,
    CRefreshLobbyList,
    CRefreshUsersOnlineList,
    CJoinLobby,
    CLeaveLobby,
    CGetBalance,
    CCheckVersion,
    CPurchaseCompleted,
    CDisconnectClient,
}