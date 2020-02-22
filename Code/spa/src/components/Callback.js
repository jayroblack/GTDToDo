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
          successCallback={(user) => {
            console.log("Success!");
            console.log(user);
            history.push('/')
          } }
          errorCallback={error => {
            console.log("ERROR BRO");
            console.log(error);
            history.push('/')
          }}
          >
          <div>Redirecting...</div>
        </CallbackComponent>
      );
    }
  }
  
  export default connect()(CallbackPage);