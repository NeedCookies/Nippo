import {
  Alert,
  Box,
  Button,
  Container,
  Snackbar,
  TextField,
  Typography,
} from "@mui/material";
import axios from "axios";
import { useState } from "react";
import { Modal } from "react-bootstrap";

interface BuyModalProps {
  courseId: number;
  courseTitle: string;
  price: number;
  handleClose: () => void;
  isOpen: boolean;
}

export const BuyModal = ({
  courseId,
  courseTitle,
  price,
  handleClose,
  isOpen,
}: BuyModalProps) => {
  const [promo, setPromo] = useState<string>("");
  const [currentPrice, setCurretPrice] = useState<number>(price);
  const [succeedSnack, setSucceedSnack] = useState<boolean>(false);
  const [promoSuccees, setPromoSuccees] = useState<boolean>(false);
  const [promoFail, setPromoFail] = useState<boolean>(false);
  const [alreadyBought, setAlreadyBought] = useState<boolean>(false);
  const [lackOfPoints, setLackOfPoints] = useState<boolean>(false);

  const handleSendPromo = async () => {
    try {
      const response = await axios.post(`/course/apply-promocode`, {
        courseId: courseId,
        promocode: promo,
      });
      if (response.status === 200) {
        setCurretPrice(response.data);
        if (response.data < price && response.data > 0) {
          setPromoFail(false);
          setSucceedSnack(true);
          setPromoSuccees(true);
        } else {
          setPromoFail(true);
          setPromoSuccees(false);
        }
      } else {
        console.log(response.status, response);
        setPromoFail(true);
        setPromoSuccees(false);
      }
    } catch (error) {
      console.error(error);
      setPromoFail(true);
      setPromoSuccees(false);
    }
  };

  const handleSnackClose = () => {
    setSucceedSnack(false);
    setPromoFail(false);
    setAlreadyBought(false);
    setLackOfPoints(false);
  };

  const handleBuyClick = async (courseId: number) => {
    try {
      const response = await axios.post(`/course/purchase-course`, {
        courseId: courseId,
        promocode: promo,
      });
      if (response.status === 200) {
        console.log("Курс успешно куплен");
        console.log("Cделай всплывающее окно с уведомлением");
      } else {
        console.log("Another response status");
        console.log(response.status);
      }
    } catch (error) {
      if (error.response.status === 500) {
        if (
          String(error.response.data).includes("Course already bought by user")
        ) {
          console.log("Вы уже купили этот курс");
          setAlreadyBought(true);
        } else if (
          String(error.response.data).includes("Don't have enough points")
        ) {
          console.log("У вас недостаточно баллов для покупки этого курса");
          setLackOfPoints(true);
        } else {
          console.error(
            "Ошибка при покупке курса:",
            error.response.data.message
          );
        }
      } else {
        console.error(
          "Ошибка при покупке курса:",
          error.response.status,
          error.response.data
        );
      }
    }
  };

  return (
    <Modal
      show={isOpen}
      onHide={handleClose}
      aria-labelledby="modal-modal-title"
      aria-describedby="modal-modal-description">
      <Container
        sx={{
          bgcolor: "#f0f0f0",
          borderRadius: "10px",
          width: "500px",
          padding: "20px",
        }}>
        <Typography variant="h4" sx={{ fontWeight: "500", color: "black" }}>
          Псс, промокод есть?
        </Typography>
        <Box>
          <Typography>Покупка курса {courseTitle}</Typography>
          <Typography>Введите промокод при наличии</Typography>
          <TextField value={promo} onChange={(e) => setPromo(e.target.value)} />
          <Button
            sx={{ margin: 1 }}
            variant="contained"
            onClick={handleSendPromo}>
            Применить
          </Button>
        </Box>
        <Box>
          <Typography
            variant="body1"
            sx={
              promoSuccees
                ? { textDecoration: "line-through", color: "red" }
                : {}
            }>
            Цена курса: {price}
          </Typography>
          {promoSuccees && (
            <>
              <Typography>Новая цена курса</Typography>
              <Typography sx={{ color: "green" }}>{currentPrice}</Typography>
            </>
          )}
        </Box>
        <Button
          variant="contained"
          color="success"
          sx={{ width: "150px" }}
          onClick={() => handleBuyClick(courseId)}>
          Купить
        </Button>
      </Container>
      <Snackbar
        open={succeedSnack}
        autoHideDuration={5000}
        onClose={handleSnackClose}>
        <Alert
          onClose={handleSnackClose}
          severity="success"
          variant="filled"
          sx={{ width: "100%" }}>
          Промокод применен
        </Alert>
      </Snackbar>
      <Snackbar
        open={promoFail}
        autoHideDuration={5000}
        onClose={handleSnackClose}>
        <Alert
          onClose={handleSnackClose}
          severity="warning"
          variant="filled"
          sx={{ width: "100%" }}>
          Не удалось применить промокод, возможно истек его срок
        </Alert>
      </Snackbar>
      <Snackbar
        open={alreadyBought}
        autoHideDuration={5000}
        onClose={handleSnackClose}>
        <Alert
          onClose={handleSnackClose}
          severity="success"
          variant="filled"
          sx={{ width: "100%" }}>
          Курс уже куплен вами
        </Alert>
      </Snackbar>
      <Snackbar
        open={lackOfPoints}
        autoHideDuration={5000}
        onClose={handleSnackClose}>
        <Alert
          onClose={handleSnackClose}
          severity="warning"
          variant="filled"
          sx={{ width: "100%" }}>
          Недостаточно поинтов для покупки
        </Alert>
      </Snackbar>
    </Modal>
  );
};
