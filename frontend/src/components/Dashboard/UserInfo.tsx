import useUserData from "@/hooks/quizes/useUserData";
import Loading from "../Status/Loading";
import Error from "../Status/Error";

interface UserInfoProps {
  username: string;
}

export default function UserInfo({ username }: UserInfoProps) {
  const { isLoading, isError, user } = useUserData(username);

  return (
    <>
      {isLoading && <Loading />}
      {isError && <Error />}
      <div>
        <div>{username}</div>
        <div>{user?.email}</div>
        <div>{user?.createdAt.slice(0, 10)}</div>
      </div>
    </>
  );
}
