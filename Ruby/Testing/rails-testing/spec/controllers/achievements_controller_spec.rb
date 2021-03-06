require 'rails_helper'

describe AchievementsController, type: :controller do

  shared_examples 'public access to achievements' do

    describe 'GET index' do

      it 'assigns only public achievements to template' do

        achievement = instance_double(Achievement)

        allow(Achievement).to receive(:get_public_achievements) { [achievement] }
        get :index
        expect(assigns(:achievements)).to eq([achievement])
      end

      it 'renders :index template' do
        get :index
        expect(response).to render_template(:index)
      end

      it 'assigns only public achievements to template' do

        public_achievement = FactoryGirl.create(:public_achievement)
        private_achievement = FactoryGirl.create(:private_achievement)

        get :index
        expect(assigns(:achievements)).to match_array([public_achievement])
      end
    end

    describe 'GET show' do

      let(:achievement) { FactoryGirl.create(:public_achievement) }

      it 'renders :show template' do
        get :show, params: {id: achievement}
        expect(response).to render_template(:show)
      end

      it 'assigns requested achievement to @achievement' do
        get :show, params: {id: achievement}
        expect(assigns(:achievement)).to eq(achievement)
      end
    end
  end

  describe 'guest user' do

    it_behaves_like 'public access to achievements'

    describe 'GET new' do
      it 'redirects to login page' do
        get :new
        expect(response).to redirect_to(new_user_session_url)
      end
    end

    describe 'POST create' do
      it 'redirects to login page' do
        post :create, params: {achievement: FactoryGirl.attributes_for(:public_achievement)}
        expect(response).to redirect_to(new_user_session_url)
      end
    end

    describe 'GET edit' do
      it 'redirects to login page' do
        get :edit, params: {id: FactoryGirl.create(:public_achievement)}
        expect(response).to redirect_to(new_user_session_url)
      end
    end

    describe 'PUT update' do
      it 'redirects to login page' do
        put :update, params: {id: FactoryGirl.create(:public_achievement), achievement: FactoryGirl.attributes_for(:public_achievement, title: 'Tytulik')}
        expect(response).to redirect_to(new_user_session_url)
      end
    end

    describe 'DELETE destroy' do
      it 'redirects to login page' do
        delete :destroy, {id: FactoryGirl.create(:public_achievement)}
        expect(response).to redirect_to(new_user_session_url)
      end
    end
  end

  describe 'authenticated user' do

    # context 'isolated level' do
    #
    #   let(:user) {instance_double(User)}
    #
    #   before do
    #     allow(controller).to receive(:current_user) {user}
    #     allow(controller).to receive(:authenticate_user!) {true}
    #   end
    #
    #   describe 'POST create' do
    #     let(:achievement_params) {{title: 'title'}}
    #     let(:create_achievement) {instance_double(CreateAchievement)}
    #
    #     before do
    #       allow(CreateAchievement).to receive(:new) {create_achievement}
    #       allow(create_achievement).to receive(:create)
    #       allow(create_achievement).to receive(:created?)
    #     end
    #
    #     it 'sends create message to CreateAchievement' do
    #       expect(CreateAchievement).to receive(:new).with(achievement_params, user)
    #       expect(create_achievement).to receive(:create)
    #       post :create, achievement: achievement_params
    #     end
    #
    #     context 'achievement is created' do
    #
    #       before {allow(create_achievement).to receive(:created?) {true}}
    #
    #       it 'redirects' do
    #         post :create, achievement: achievement_params
    #         expect(response.status).to eq(302)
    #       end
    #     end
    #   end
    # end

    context 'controller level' do

      let(:user) { FactoryGirl.create(:user) }

      before do
        sign_in(user)
      end

      it_behaves_like 'public access to achievements'

      describe 'GET new' do
        it 'renders :new template' do
          get :new
          expect(response).to render_template(:new)
        end

        it 'assigns new Achievement object to @achievement' do
          get :new
          expect(assigns(:achievement)).to be_a_new(Achievement)
        end
      end

      describe 'POST create' do

        let(:valid_data) { FactoryGirl.attributes_for(:public_achievement) }

        context 'valid data' do
          it 'redirect to achievements#show route' do
            post :create, params: {achievement: valid_data}
            expect(response).to redirect_to(achievement_path(assigns[:achievement]))
          end
          it 'creates new achievement id database' do
            expect {
              post :create, params: {achievement: valid_data}
            }.to change(Achievement, :count).by(1)
          end
        end

        context 'invalid data' do

          let(:invalid_data) { FactoryGirl.attributes_for(:public_achievement, title: '') }

          it 'renders :new template' do
            post :create, params: {achievement: invalid_data}
            expect(response).to render_template(:new)
          end
          it 'doesn\'t create new achievement in the database' do
            expect {
              post :create, params: {achievement: invalid_data}
            }.not_to change(Achievement, :count)
          end
        end
      end

      context 'user is not the owner of the achievement' do
        describe 'GET edit' do
          it 'redirects to achievements page' do
            get :edit, params: {id: FactoryGirl.create(:public_achievement)}
            expect(response).to redirect_to(achievement_path)
          end
        end

        describe 'PUT update' do
          it 'redirects to achievements page' do
            put :update, params: {id: FactoryGirl.create(:public_achievement), achievement: FactoryGirl.attributes_for(:public_achievement, title: 'Tytulik')}
            expect(response).to redirect_to(achievement_path)
          end
        end

        describe 'DELETE destroy' do
          it 'redirects to achievements page' do
            delete :destroy, {id: FactoryGirl.create(:public_achievement)}
            expect(response).to redirect_to(achievement_path)
          end
        end
      end

      context 'user is the owner of the achievement' do

        let(:achievement) { FactoryGirl.create(:public_achievement, user: user) }

        describe 'PUT update' do

          context 'valid data' do

            let(:valid_data) { FactoryGirl.attributes_for(:public_achievement, title: 'Tytulik') }

            it 'redirects to achievements#show action' do
              put :update, params: {achievement: valid_data, id: achievement}
              expect(response).to redirect_to(achievement)
            end

            it 'updates achievement in the database' do
              put :update, params: {achievement: valid_data, id: achievement}
              achievement.reload
              expect(achievement.title).to eq('Tytulik')
            end
          end

          context 'invalid data' do

            let(:invalid_data) { FactoryGirl.attributes_for(:public_achievement, title: '', description: 'meow') }

            it 'renders :edit template' do
              put :update, params: {achievement: invalid_data, id: achievement}
              expect(response).to render_template(:edit)
            end
            it 'doesnt update achievement in the database' do
              put :update, params: {achievement: invalid_data, id: achievement}
              achievement.reload
              expect(achievement.description).not_to eq('meow')
            end
          end
        end

        describe 'GET edit' do

          it 'renders :edit template' do
            get :edit, params: {id: achievement}
            expect(response).to render_template(:edit)
          end
          it 'assigns the requested achievement to template' do
            get :edit, params: {id: achievement}
            expect(assigns(:achievement)).to eq(achievement)
          end
        end

        describe 'DELETE destroy' do

          it 'redirects to achievements#index' do
            delete :destroy, params: {id: achievement}
            expect(response).to redirect_to(achievement_path)
          end
          it 'deletes achievement from the database' do
            delete :destroy, params: {id: achievement}
            expect(Achievement.exists?(achievement.id)).to be_falsey
          end
        end
      end
    end

  end
end