import { Avatar, Button } from "@mui/material"
import "./PersonalAccount.css"
import { useEffect, useState } from "react"
import axios from "axios"
import { PointsModal } from "./PointsModal";
import { Link } from "react-router-dom";

interface UserInfo {
    firstName: string;
    lastName: string;
    userName: string;
    phoneNumber: string;
    email: string;
    pictureUrl: string | null;
    birthDate: string;
    points: number;
    role: string;
}

export const PersonalAccount = () => {
    const [userInfo, setUserInfo] = useState<UserInfo | null>(null);

    const [isModalOpen, setModalOpen] = useState<boolean>(false);
    const handleOpenModal = () => setModalOpen(true);
    const handleCloseModal = () => setModalOpen(false);

    useEffect(() => {
        const getUserInfo = async() => {
            try{
                const response = await axios.get("user/get-personal-info");

                if (response.status == 200){
                    setUserInfo(response.data)
                }
                else{
                    console.log(response.status)
                }
            }
            catch(e){
                //TODO удалить, когда будет готов endpoint на сервере
                setUserInfo(
                    {
                        firstName: "Иван",
                        lastName: "Иванов",
                        userName: "TestUser",
                        phoneNumber: "+7 952 812 52 52",
                        email: "testUser@mail.ru",
                        pictureUrl: null,
                        birthDate: "2004-06-09",
                        points: 400,
                        role: "admin"
                    }
                )
                console.log(e)
            }
        }

        getUserInfo();
    }, []);

    return (
        <>
            <div className="profile-page-container">
                {userInfo && (
                    <div className="user-info-container">
                        <div className="avatar-wrapper">
                            <Avatar src={userInfo?.pictureUrl || undefined} sx={{ width: "230px", height: "230px"}}/>
                            <div className="username-wrapper">
                                {userInfo?.userName}
                            </div>
                        </div>
                        <div className="personal-info-wrapper">
                            <div className="fullname">
                                {userInfo?.firstName + " " + userInfo?.lastName}
                            </div>
                            
                            <div className="info-item">
                                <span className="info-label">Дата рождения:</span>
                                <span className="info-value">{userInfo?.birthDate}</span>
                            </div>
                            <div className="info-item">
                                <span className="info-label">Телефон:</span>
                                <span className="info-value">{userInfo?.phoneNumber}</span>
                            </div>
                            <div className="info-item">
                                <span className="info-label">Почта:</span>
                                <span className="info-value">{userInfo?.email}</span>
                            </div>
                        </div>
                        <div className="points-wrapper">
                            <div className="points">
                                {userInfo?.points}
                            </div>
                            {userInfo.role == "admin" && (
                                <div> 
                                    <Button variant="contained" 
                                        onClick={handleOpenModal}
                                        sx={{marginTop: "1rem", padding: "0.8rem"}}>
                                            Выдать поинты
                                    </Button>
                                </div>
                            )}
                        </div>
                    </div>
                )}
                <div className="profile-navigation">
                    <Link to={"/profile/stats"} className="profile-nav-link">
                        <button className="nav-button">
                            ЛИЧНАЯ СТАТИСТИКА
                        </button>
                    </Link>
                    <Link to={"/courses/favourite"} className="profile-nav-link">
                        <button className="nav-button">
                            ИЗБРАННЫЕ КУРСЫ
                        </button>
                    </Link>
                    <Link to={"/courses/bought"} className="profile-nav-link">
                        <button className="nav-button">
                            КУПЛЕННЫЕ КУРСЫ
                        </button>
                    </Link>
                    { userInfo && (userInfo.role == 'author' || userInfo.role == 'admin') && (
                        <Link to={"/author/courses"} className="profile-nav-link">
                            <button className="nav-button">
                                {"СТАСТИСТИКА\n МОИХ КУРСОВ"}
                            </button>
                        </Link>
                    )}
                    { userInfo && userInfo.role == 'admin' && (
                        <Link to={"/admin/courses"} className="profile-nav-link">
                            <button className="nav-button">
                                МОДЕРАЦИЯ КУРСОВ
                            </button>
                        </Link>
                    )}
                </div>
            </div>
            <div>
                <PointsModal open={isModalOpen} onClose={handleCloseModal} />
            </div>
        </>
    )
}