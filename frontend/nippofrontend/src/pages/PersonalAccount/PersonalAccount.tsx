import { Avatar, Button, Typography } from "@mui/material";
import "./PersonalAccount.css";
import { useEffect, useState } from "react";
import axios from "axios";
import { PointsModal } from "./PointsModal";
import { Link } from "react-router-dom";
import { EditUserInfoModal, UserInfoEdit } from "./EditUserInfoModal";
import { PromoModal } from "./PromoModal";

interface UserInfo {
  firstName: string | null;
  lastName: string | null;
  userName: string;
  phoneNumber: string | null;
  email: string;
  pictureUrl: string | null;
  birthDate: string | null;
  points: number;
  role: string;
}

export const PersonalAccount = () => {
  const [userInfo, setUserInfo] = useState<UserInfo | null>(null);

  const [isPointsModalOpen, setPointsModalOpen] = useState<boolean>(false);
  const handleOpenPointsModal = () => setPointsModalOpen(true);
  const handleClosePointsModal = () => setPointsModalOpen(false);

  const [isPromoModalOpen, setPromoModalOpen] = useState<boolean>(false);
  const handleOpenPromoModal = () => setPromoModalOpen(true);
  const handleClosePromoModal = () => setPromoModalOpen(false);

  const [isEditOpen, setEditOpen] = useState<boolean>(false);
  const handleOpenEdit = () => setEditOpen(true);
  const handleCloseEdit = () => setEditOpen(false);

  const handleUserInfoUpdate = (updatedUserInfo: UserInfoEdit) => {
    setUserInfo((prev) => ({
      ...prev!,
      firstName: updatedUserInfo.firstName,
      lastName: updatedUserInfo.lastName,
      phoneNumber: updatedUserInfo.phoneNumber,
      birthDate: updatedUserInfo.birthDate,
      pictureUrl: updatedUserInfo.pictureUrl,
    }));
  };

  const handleUserRoleUpdate = async () => {
    try {
      const response = await axios.post(`/user/upgrade-to-author`);

      if (response.status === 200) {
        getUserInfo();
      } else {
        console.log(response.status);
      }
    } catch (error) {
      console.error(error);
    }
  };

  const userInfoEdit: UserInfoEdit | null = userInfo
    ? {
        firstName: userInfo.firstName ?? "",
        lastName: userInfo.lastName ?? "",
        phoneNumber: userInfo.phoneNumber ?? "",
        pictureUrl: userInfo.pictureUrl,
        birthDate: userInfo.birthDate,
      }
    : null;
  const getUserInfo = async () => {
    try {
      const response = await axios.get("user/get-personal-info");

      if (response.status == 200) {
        setUserInfo(response.data);
      } else {
        console.log(response.status);
      }
    } catch (e) {
      console.log(e);
    }
  };
  useEffect(() => {
    getUserInfo();
  }, []);

  return (
    <>
      <div className="profile-page-container">
        {userInfo && (
          <div className="user-info-container">
            <div className="avatar-wrapper">
              <Avatar
                src={userInfo?.pictureUrl || undefined}
                sx={{ width: "230px", height: "230px" }}
              />
              <div className="username-wrapper">{userInfo.userName}</div>
            </div>
            <div className="personal-info-wrapper">
              <div className="fullname">
                {userInfo.firstName && userInfo.lastName
                  ? userInfo.firstName + " " + userInfo.lastName
                  : "Не заполнено"}
              </div>
              <div className="info-item">
                <span className="info-label">Дата рождения:</span>
                <span className="info-value">
                  {userInfo.birthDate ?? "Не заполнено"}
                </span>
              </div>
              <div className="info-item">
                <span className="info-label">Телефон:</span>
                <span className="info-value">
                  {userInfo.phoneNumber ?? "Не заполнено"}
                </span>
              </div>
              <div className="info-item">
                <span className="info-label">Почта:</span>
                <span className="info-value">{userInfo.email}</span>
              </div>
              <div className="info-item">
                <span className="info-label">Статус:</span>
                <span className="info-value">{userInfo.role}</span>
              </div>
              <div>
                <Button variant="contained" onClick={handleOpenEdit}>
                  Редактировать
                </Button>
              </div>
              {userInfo && userInfo.role == "user" && (
                <div className="flex">
                  <Typography
                    sx={{ fontFamily: "cursive", fontStyle: "italic" }}>
                    Чтобы стать автором курса, вам нужно заполнить всю
                    информацию о себе
                  </Typography>
                  <Button
                    variant="contained"
                    disabled={
                      userInfo.firstName == undefined ||
                      userInfo.lastName == undefined ||
                      userInfo.birthDate == undefined ||
                      userInfo.phoneNumber == undefined ||
                      userInfo.pictureUrl == undefined
                    }
                    onClick={handleUserRoleUpdate}>
                    Стать автором
                  </Button>
                </div>
              )}
            </div>
            <div className="points-wrapper">
              <div className="points">{userInfo.points}</div>
              {userInfo.role == "admin" && (
                <div>
                  <Button
                    variant="contained"
                    onClick={handleOpenPointsModal}
                    sx={{ marginTop: "1rem", padding: "0.8rem" }}>
                    Выдать поинты
                  </Button>
                </div>
              )}
            </div>
          </div>
        )}
        <div className="profile-navigation">
          <Link to={"/profile/stats"} className="profile-nav-link">
            <button className="nav-button">ЛИЧНАЯ СТАТИСТИКА</button>
          </Link>
          <Link to={"/basket"} className="profile-nav-link">
            <button className="nav-button">ИЗБРАННЫЕ КУРСЫ</button>
          </Link>
          <Link to={"/my-courses"} className="profile-nav-link">
            <button className="nav-button">КУПЛЕННЫЕ КУРСЫ</button>
          </Link>
          {userInfo &&
            (userInfo.role == "author" || userInfo.role == "admin") && (
              <>
                <Link to={"/author/courses"} className="profile-nav-link">
                  <button className="nav-button">
                    {"СТАСТИСТИКА\n МОИХ КУРСОВ"}
                  </button>
                </Link>
                <Link to={"/course/create"} className="profile-nav-link">
                  <button className="nav-button">{"СОЗДАТЬ КУРС"}</button>
                </Link>
              </>
            )}
          {userInfo && userInfo.role == "admin" && (
            <div>
              <Link to={"/admin/courses"} className="profile-nav-link">
                <button className="nav-button">МОДЕРАЦИЯ КУРСОВ</button>
              </Link>
              <button className="nav-button" onClick={handleOpenPromoModal}>
                Создать промокод
              </button>
            </div>
          )}
        </div>
      </div>
      <div>
        <PromoModal open={isPromoModalOpen} onClose={handleClosePromoModal} />
      </div>
      <div>
        <PointsModal
          open={isPointsModalOpen}
          onClose={handleClosePointsModal}
        />
      </div>
      <div>
        <EditUserInfoModal
          open={isEditOpen}
          onClose={handleCloseEdit}
          userInfo={userInfoEdit}
          onUserInfoUpdate={handleUserInfoUpdate}
        />
      </div>
    </>
  );
};
