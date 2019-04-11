public enum ServerPackages {
    SWelcomeMsg = 1,
    SAccountExist,
    SLobbyCreated,
    SSendLobbyList,
    SPlayerJoined,
    SMatchOver,
}

public enum ClientPackages {
    CThankYouMsg = 1,
    CCheckForAccount,
    CCreateAccount,
    CGameOver,
    CCreateLobby,
    CRefreshLobbyList,
    CJoinLobby,
}