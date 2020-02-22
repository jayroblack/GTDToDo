import React from "react";
import { connect } from "react-redux";
import { CallbackComponent } from "redux-oidc";
import userManager from "../userManager";
import history from "../history";

class CallbackPage extends React.Component {
    render() {
      // just redirect to '/' in both cases
      return (
        <CallbackComponent
          userManager={userManager}
          successCallback={() => history.push('/')}
          errorCallback={error => {
            history.push('/')
            console.error(error);
          }}
          >
          <div>Redirecting...</div>
        </CallbackComponent>
      );
    }
  }
  
  export default connect()(CallbackPage);