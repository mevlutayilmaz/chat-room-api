import React from "react";
import { FaFacebookF, FaTwitter, FaGoogle } from "react-icons/fa";
import { useForm } from 'react-hook-form';
import { login } from '../../api/auth';
import AuthService from "../../services/AuthService" 

const Login = () => {
  const { register, handleSubmit } = useForm();

  const onSubmit = async (data) => {
    const response = await login(data.username, data.password);
    if(response.status === 200){
        AuthService.setToken(response.data);
        AuthService.setUsername(data.username)
        window.location.href = "/";
    }
  }

  return (
    <section className="bg-gradient-primary">
      <div className="container">
        <div className="row justify-content-center align-items-center">
          <div className="col-lg-6 col-md-8 col-12">
            <div className="card shadow-lg border-0 rounded-4">
              <div className="card-body p-5">
                <h4 className="text-center mb-4 font-weight-bold text-uppercase text-dark">
                  Login
                </h4>

                <form className="space-y-6" onSubmit={handleSubmit(onSubmit)}>
                  <div className="form-group mb-4">
                    <label htmlFor="username" className="form-label">
                      Username
                    </label>
                    <input
                      type="text"
                      {...register('username', { required: 'Username is required' })}
                      id="username"
                      className="form-control form-control"
                      placeholder="username123"
                      required
                    />
                  </div>

                  <div className="form-group mb-4">
                    <label htmlFor="password" className="form-label">
                      Password
                    </label>
                    <input
                      type="password"
                      {...register('password', { required: 'Password is required' })}
                      id="password"
                      className="form-control form-control"
                      placeholder="••••••••"
                      required
                    />
                  </div>

                  <div className="d-flex justify-content-between align-items-center">
                    <button type="submit" className="btn btn-primary btn-lg w-100">
                      Login
                    </button>
                  </div>
                </form>

                <div className="text-center">
                  <p className="text-muted mb-2">Or login with</p>
                  <div className="d-flex justify-content-center">
                    <a href="#" className="text-dark mx-2">
                      <FaFacebookF size={30} />
                    </a>
                    <a href="#" className="text-dark mx-2">
                      <FaTwitter size={30} />
                    </a>
                    <a href="#" className="text-dark mx-2">
                      <FaGoogle size={30} />
                    </a>
                  </div>
                </div>

                <div className="text-center mt-4">
                  <p className="mb-0">
                    Don't have an account?{" "}
                    <a href="/signup" className="text-primary">
                      Signup
                    </a>
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
};

export default Login;
