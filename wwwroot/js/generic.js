window.copyInviteLink = function (lobbyName) {
    navigator.clipboard.writeText(`${window.location.origin}/lobby/${encodeURIComponent(lobbyName)}`);
}
