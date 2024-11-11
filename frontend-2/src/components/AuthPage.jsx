import { useEffect, useState } from "react";
import { useNavigate, useOutletContext } from "react-router-dom";

export function AuthPage() {
  const { setAuthorized } = useOutletContext();
  const navigate = useNavigate();
  const [haveAccount, setHaveAccount] = useState(false);
  const [passwordShown, setPasswordShown] = useState(false);

  function handleAuthentication() {
    setAuthorized(true);
    navigate("/");
  }

  useEffect(() => {}, []);

  return (
    <section
      id="auth-page"
      className="min-h-screen flex flex-col bg-gradient-to-r from-slate-800 to-slate-950 text-white text-sm sm:text-base items-center justify-center">
      <div className="flex flex-col justify-center items-center w-full sm:max-w-sm bg-slate-600 p-6 rounded-lg gap-4">
        <img
          src="./logo.jpg"
          className="w-[40px] sm:w-[60px] md:w-[80px] rounded-full"></img>
        <h2 className="text-2xl font-semibold">
          {haveAccount ? "Вход в профиль" : "Регистрация"}
        </h2>
        <input
          placeholder="Электронная почта"
          className="text-slate-950 w-full rounded p-2 bg-slate-50"></input>
        {!haveAccount && (
          <input
            placeholder="Ваше имя"
            className="text-slate-950 w-full rounded p-2 bg-slate-50"></input>
        )}
        <div className="flex relative items-center w-full">
          <input
            placeholder="Пароль"
            type={passwordShown ? "text" : "password"}
            className="text-slate-950 w-full rounded p-2 bg-slate-50"></input>
          <button
            onClick={() => setPasswordShown(!passwordShown)}
            className="text-black absolute right-2">
            {passwordShown ? (
              <i className="fa-regular fa-eye-slash"></i>
            ) : (
              <i className="fa-regular fa-eye"></i>
            )}
          </button>
        </div>
        <button
          className="font-semibold bg-slate-800 hover:bg-slate-950 p-2 rounded w-full"
          onClick={handleAuthentication}>
          {haveAccount ? "Войти" : "Создать профиль"}
        </button>
        <button
          onClick={() => setHaveAccount(!haveAccount)}
          className="underline hover:decoration-2">
          {haveAccount ? "Еще нет аккаунта?" : "Уже есть аккаунт"}
        </button>
      </div>
    </section>
  );
}
