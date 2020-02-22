import React from "react";
import { connect } from "react-redux";

class HomePage extends React.Component {
    render(){
        return (
            <div>This is the Home Page</div>
        );
    }
}

function mapStateToProps(state) {
    return {
      user: state.oidc.user
    };
  }

function mapDispatchToProps(dispatch) {
    return {
      dispatch
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(HomePage);