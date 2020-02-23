import React from 'react'
import { Route, Redirect } from 'react-router-dom'

// Need to learn how to use Hooks first.  
const PrivateRoute = ({ component: Component, ...props }) => {
    return (
      <Route
        {...props}
        render={innerProps =>
          myAuth.isAuth ? 
              <Component {...innerProps} />
              :
              <Redirect to="/" />
        }
      />
    );
  };