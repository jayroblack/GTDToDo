import { createUserManager } from 'redux-oidc';

const userManagerConfig = {
    authority: "http://localhost:5000",
    client_id: "js",
    redirect_uri: "http://localhost:3000/callback",
    response_type: "code",
    scope: "openid profile api1 email address phone",
    post_logout_redirect_uri: "http://localhost:3000/",
    loadUserInfo: true
  };
  
  const userManager = createUserManager(userManagerConfig);
  
  export default userManager;