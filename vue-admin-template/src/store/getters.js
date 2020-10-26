const getters = {
  sidebar: state => state.app.sidebar,
  device: state => state.app.device,
  token: state => state.user.token,
  refreshToken: state => state.user.refreshToken,
  expires: state => state.user.expires,
  refreshExpires: state => state.user.refreshExpires,
  avatar: state => state.user.avatar,
  name: state => state.user.name
}
export default getters
