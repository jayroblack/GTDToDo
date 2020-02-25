import React from 'react'
import { Route, Redirect } from 'react-router-dom'
import { useSelector } from 'react-redux'

// Need to learn how to use Hooks first.  
const PrivateRoute = ({ component: Component, ...props }) => {
  const user = useSelector(state => state.oidc.user)
    return (
      <Route
        {...props}
        render={innerProps =>
          user ? 
              <Component {...innerProps} />
              :
              <Redirect to="/" />
        }
      />
    );
  };

  export default PrivateRoute