class AuthService {
  static getUsername() {
    const username = localStorage.getItem("username");
    return username ? username : null;
  }

  static setUsername(username) {
    localStorage.setItem("username", username);
  }

  static getToken() {
    const token = localStorage.getItem("authToken");
    return token ? JSON.parse(token) : null;
  }

  static setToken(token) {
    localStorage.setItem("authToken", JSON.stringify(token));
  }

  static getAccessToken() {
    return this.isTokenExpired()
      ? null
      : this.getToken().accessToken;
  }

  static getExpiration() {
    const token = this.getToken();
    return token ? new Date(token.expiration) : null;
  }

  static isTokenExpired() {
    const expiration = this.getExpiration();
    if (expiration) return new Date() > expiration;
    return true;
  }

  static logout() {
    localStorage.removeItem("authToken");
  }
}

export default AuthService;